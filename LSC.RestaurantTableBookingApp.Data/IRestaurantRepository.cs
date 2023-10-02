using LSC.RestaurantTableBookingApp.Core;
using LSC.RestaurantTableBookingApp.Core.ViewModels;

namespace LSC.RestaurantTableBookingApp.Data
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<RestaurantModel>> GetAllRestaurantsAsync();
        Task<IEnumerable<RestaurantBranchModel>> GetRestaurantBranchsByRestaurantIdAsync(int restaurantId);

        /// <summary>
        /// LINQ query retrieves dining tables and their associated time slots for a specific branchId and date. The result is sorted by Id and then MealType. The data is then projected into a list of DiningTableWithTimeSlotsModel view models and returned.
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<IEnumerable<DiningTableWithTimeSlotsModel>> GetDiningTablesByBranchAsync(int branchId, DateTime date);

        /// <summary>
        /// LINQ query retrieves dining tables and their associated time slots for a specific branchId and date which are current and future. The result is sorted by Id and then MealType. The data is then projected into a list of DiningTableWithTimeSlotsModel view models and returned.
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<IEnumerable<DiningTableWithTimeSlotsModel>> GetDiningTablesByBranchAsync(int branchId);

        Task<RestaurantReservationDetails> GetRestaurantReservationDetailsAsync(int timeSlotId);

        Task<User?> GetUserAsync(string emailId);
    }
}
