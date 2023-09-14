﻿using LSC.RestaurantTableBookingApp.Core;
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
                ReservationDate = reservation.ReservationDate,
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
    }
}