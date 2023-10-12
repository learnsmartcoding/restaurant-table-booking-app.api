using LSC.RestaurantTableBookingApp.Core;
using LSC.RestaurantTableBookingApp.Core.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LSC.RestaurantTableBookingApp.Data
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly RestaurantTableBookingDbContext _dbContext;
        public ReservationRepository(RestaurantTableBookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateOrUpdateReservationAsync(ReservationModel reservation)
        {
            var mealType = _dbContext.TimeSlots.First(f=>f.Id==reservation.TimeSlotId).MealType;
            // Define the time for each meal type
            TimeSpan breakfastTime = new TimeSpan(8, 0, 0); // 8:00 AM
            TimeSpan lunchTime = new TimeSpan(12, 0, 0);   // 12:00 PM
            TimeSpan dinnerTime = new TimeSpan(18, 0, 0);  // 6:00 PM

            // Based on the meal type, set the time for ReservationDate
            TimeSpan reservationTime;

            if (mealType == "Breakfast")
            {
                reservationTime = breakfastTime;
            }
            else if (mealType == "Lunch")
            {
                reservationTime = lunchTime;
            }
            else if (mealType == "Dinner")
            {
                reservationTime = dinnerTime;
            }
            else
            {
                throw new ArgumentException("Invalid meal type.");
            }

            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.FirstName == reservation.FirstName
            && u.LastName == reservation.LastName && u.Email == reservation.EmailId);

            if (existingUser == null)
            {
                var newUser = new User
                {
                    FirstName = reservation.FirstName,
                    LastName = reservation.LastName,
                    Email = reservation.EmailId,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                _dbContext.Users.Add(newUser);
                await _dbContext.SaveChangesAsync();

                existingUser = newUser;
            }
            var timeslotToUpdate = await _dbContext.TimeSlots.FindAsync(reservation.TimeSlotId);
            timeslotToUpdate.TableStatus = "Booked";
            _dbContext.TimeSlots.Update(timeslotToUpdate);

            var newReservation = new Reservation
            {
                UserId = existingUser.Id,
                TimeSlotId = reservation.TimeSlotId,
                ReservationDate = reservation.ReservationDate.Date.Add(reservationTime),
                ReservationStatus = reservation.ReservationStatus
            };

            _dbContext.Reservations.Add(newReservation);
            await _dbContext.SaveChangesAsync();

            return newReservation.Id;
        }
        public async Task<TimeSlot> GetTimeSlotByIdAsync(int timeSlotId)
        {
            var timeSlot = await _dbContext.TimeSlots.FindAsync(timeSlotId);
            return timeSlot;
        }

        public async Task<DiningTableWithTimeSlotsModel> UpdateReservationAsync(DiningTableWithTimeSlotsModel reservation)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == reservation.UserEmailId);

            var timeslotToUpdate = await _dbContext.TimeSlots.FindAsync(reservation.TimeSlotId);
            timeslotToUpdate.TableStatus = "Checked In";

            _dbContext.TimeSlots.Update(timeslotToUpdate);

            var reservationToUpdate = await _dbContext.Reservations.FirstAsync(f => f.TimeSlotId == reservation.TimeSlotId);
            reservationToUpdate.ReservationStatus = "Checked In";

            _dbContext.Reservations.Update(reservationToUpdate);
            await _dbContext.SaveChangesAsync();

            return reservation;
        }

        public async Task<List<ReservationDetailsModel>> GetReservationDetailsAsync()
        {
            var reservationDetails = await (
                 from dt in _dbContext.DiningTables
                 join r in _dbContext.Reservations on dt.Id equals r.Id
                 join rb in _dbContext.RestaurantBranches on dt.RestaurantBranchId equals rb.Id
                 join rt in _dbContext.Restaurants on rb.RestaurantId equals rt.Id
                 join ts in _dbContext.TimeSlots on r.TimeSlotId equals ts.Id
                 join u in _dbContext.Users on r.UserId equals u.Id
                 //where r.ReservationDate == DateTime.Now.Date
                 select new ReservationDetailsModel
                 {
                     Name = rt.Name,
                     BranchName = rb.Name,
                     Address = rb.Address,
                     Phone = rb.Phone,
                     TableName = dt.TableName,
                     Capacity = dt.Capacity,
                     ReservationDate = r.ReservationDate,
                     MealType = ts.MealType,
                     TableStatus = ts.TableStatus,
                     ReservationStatus = r.ReservationStatus,
                     FirstName = u.FirstName,
                     LastName = u.LastName,
                     Email = u.Email
                 }
             ).ToListAsync() ;


            return reservationDetails;

        }
    }
}
