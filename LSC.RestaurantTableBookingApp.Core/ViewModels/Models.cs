using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LSC.RestaurantTableBookingApp.Core.ViewModels
{
    public partial class RestaurantModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class RestaurantBranchModel
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class DiningTableWithTimeSlotsModel
    {
        public int BranchId { get; set; }
        public DateTime ReservationDay { get; set; }
        public string? TableName { get; set; }
        public int Capacity { get; set; }
        public string MealType { get; set; } = null!;
        public string TableStatus { get; set; } = null!;
        public int TimeSlotId { get; set; }
        public string? UserEmailId { get; set; }
    }

    public class ReservationModel
    {
        public string? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public required string EmailId { get; set; }
        public string? PhoneNumber { get; set; }
        public required int TimeSlotId { get; set; }
        public required DateTime ReservationDate { get; set; }
        public string ReservationStatus { get; set; }
    }
    public class RestaurantReservationDetails
    {
        public string RestaurantName { get; set; }
        public string BranchName { get; set; }
        public string TableName { get; set; }
        public int Capacity { get; set; }
        public string MealType { get; set; }
        public DateTime ReservationDay { get; set; }
        public string Address { get; set; }
    }
    public class ReservationDetailsModel
    {
        public string Name { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string TableName { get; set; }
        public int Capacity { get; set; }
        public DateTime ReservationDate { get; set; }
        public string MealType { get; set; }
        public string TableStatus { get; set; }
        public string ReservationStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

}
