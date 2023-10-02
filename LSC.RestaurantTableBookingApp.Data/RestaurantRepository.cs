using LSC.RestaurantTableBookingApp.Core;
using LSC.RestaurantTableBookingApp.Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace LSC.RestaurantTableBookingApp.Data
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantTableBookingDbContext _dbContext;

        
        public RestaurantRepository(RestaurantTableBookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<User?> GetUserAsync(string emailId)
        {
            return _dbContext.Users.FirstOrDefaultAsync(f=>f.Email.Equals(emailId));
        }

        public async Task<RestaurantReservationDetails> GetRestaurantReservationDetailsAsync(int timeSlotId)
        {           

            var query = await (from diningTable in _dbContext.DiningTables
                        join restaurantBranch in _dbContext.RestaurantBranches on diningTable.RestaurantBranchId equals restaurantBranch.Id
                        join restaurant in _dbContext.Restaurants on restaurantBranch.RestaurantId equals restaurant.Id
                        join timeSlot in _dbContext.TimeSlots on diningTable.Id equals timeSlot.DiningTableId
                        where timeSlot.Id == timeSlotId
                        select new RestaurantReservationDetails()
                        {
                            RestaurantName= restaurant.Name,
                            BranchName = restaurantBranch.Name,
                            Address= restaurantBranch.Address,
                            TableName= diningTable.TableName,
                           Capacity= diningTable.Capacity,
                            MealType = timeSlot.MealType,
                            ReservationDay = timeSlot.ReservationDay                            
                        }).FirstOrDefaultAsync();

            return query;
        }
        public async Task<IEnumerable<RestaurantModel>> GetAllRestaurantsAsync()
        {
            var restaurants = await _dbContext.Restaurants
                .Select(r => new RestaurantModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Address = r.Address,
                    Phone = r.Phone,
                    Email = r.Email,
                    ImageUrl = r.ImageUrl
                })
                .ToListAsync();

            return restaurants;
        }

        /// <summary>
        /// LINQ query retrieves dining tables and their associated time slots for a specific branchId and date. The result is sorted by Id and then MealType. The data is then projected into a list of DiningTableWithTimeSlotsModel view models and returned.
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DiningTableWithTimeSlotsModel>> GetDiningTablesByBranchAsync(int branchId, DateTime date)
        {
            var diningTables = await _dbContext.DiningTables
             .Where(dt => dt.RestaurantBranchId == branchId)
             .SelectMany(dt => dt.TimeSlots, (dt, ts) => new
             {
                 dt.RestaurantBranchId,
                 dt.TableName,
                 dt.Capacity,
                 ts.ReservationDay,
                 ts.MealType,
                 ts.TableStatus,
                 ts.Id
             })
             .Where(ts => ts.ReservationDay.Date == date.Date)
             .OrderBy(ts => ts.Id)
             .ThenBy(ts => ts.MealType)
             .ToListAsync();

            return diningTables.Select(dt => new DiningTableWithTimeSlotsModel
            {
                BranchId = dt.RestaurantBranchId,
                ReservationDay = dt.ReservationDay.Date,
                TableName = dt.TableName,
                Capacity = dt.Capacity,
                MealType = dt.MealType,
                TableStatus = dt.TableStatus,
                TimeSlotId = dt.Id
            });
            /*
             Let's break down the query step by step:

var diningTables = await _dbContext.DiningTables: This starts the query by selecting all entities from the DiningTables table.

.Where(dt => dt.RestaurantBranchId == branchId): This filters the DiningTables entities to only those where the RestaurantBranchId matches the provided branchId.

.SelectMany(dt => dt.TimeSlots, (dt, ts) => new { ... }): This performs a "flattening" operation using the SelectMany method. It associates each DiningTable with its corresponding TimeSlots and creates a new anonymous object with properties from both DiningTable (dt) and TimeSlots (ts). The new object will contain properties like RestaurantBranchId, TableName, Capacity, ReservationDay, MealType, TableStatus, and Id (which represents the TimeSlotId).

.Where(ts => ts.ReservationDay.Date == date.Date): This further filters the flattened result by matching only those records where the ReservationDay matches the date.Date. It's comparing only the date part of the ReservationDay property.

.OrderBy(ts => ts.Id).ThenBy(ts => ts.MealType): This sorts the filtered result first by Id in ascending order, and then within each Id group, it sorts by MealType in ascending order.

.ToListAsync(): This asynchronously executes the query and retrieves the result as a list.

return diningTables.Select(dt => new DiningTableWithTimeSlotsModel { ... }): Finally, after receiving the list of anonymous objects, it maps the data to a list of DiningTableWithTimeSlotsModel objects using the Select method. The DiningTableWithTimeSlotsModel is the view model that will be returned from the method, and the properties are assigned from the corresponding properties in the anonymous object.

In summary, this LINQ query retrieves dining tables and their associated time slots for a specific branchId and date. The result is sorted by Id and then MealType. The data is then projected into a list of DiningTableWithTimeSlotsModel view models and returned.
             * */
        }

        public async Task<IEnumerable<DiningTableWithTimeSlotsModel>> GetDiningTablesByBranchAsync(int branchId)
        {
            var data = await (
                from rb in _dbContext.RestaurantBranches
                join dt in _dbContext.DiningTables on rb.Id equals dt.RestaurantBranchId
                join ts in _dbContext.TimeSlots on dt.Id equals ts.DiningTableId
                where dt.RestaurantBranchId == branchId && ts.ReservationDay >= DateTime.Now.Date
                orderby ts.Id, ts.MealType
                select new DiningTableWithTimeSlotsModel()
                {
                    BranchId = rb.Id,
                    Capacity = dt.Capacity,
                    TableName = dt.TableName,
                    MealType = ts.MealType,
                    ReservationDay = ts.ReservationDay,
                    TableStatus = ts.TableStatus,
                    TimeSlotId = ts.Id,
                    UserEmailId = (from r in _dbContext.Reservations
                                   join u in _dbContext.Users on r.UserId equals u.Id
                                   where r.TimeSlotId == ts.Id
                                   select u.Email.ToLower()).FirstOrDefault()
                }).ToListAsync();


            return data;
        }

        public async Task<IEnumerable<RestaurantBranchModel>> GetRestaurantBranchsByRestaurantIdAsync(int restaurantId)
        {
            var branches = await _dbContext.RestaurantBranches
                .Where(rb => rb.RestaurantId == restaurantId)
                .Select(rb => new RestaurantBranchModel
                {
                    Id = rb.Id,
                    RestaurantId = rb.RestaurantId,
                    Name = rb.Name,
                    Address = rb.Address,
                    Phone = rb.Phone,
                    Email = rb.Email,
                    ImageUrl = rb.ImageUrl
                })
                .ToListAsync();

            return branches;
        }

       



    }

}
