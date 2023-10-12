using LSC.RestaurantTableBookingApp.Core;
using LSC.RestaurantTableBookingApp.Core.ViewModels;

namespace LSC.RestaurantTableBookingApp.Data
{
    public interface IReservationRepository
    {
        Task<int> CreateOrUpdateReservationAsync(ReservationModel reservation);
        Task<TimeSlot> GetTimeSlotByIdAsync(int timeSlotId);

        Task<DiningTableWithTimeSlotsModel> UpdateReservationAsync(DiningTableWithTimeSlotsModel reservation);
        Task<List<ReservationDetailsModel>> GetReservationDetailsAsync();
    }
}
