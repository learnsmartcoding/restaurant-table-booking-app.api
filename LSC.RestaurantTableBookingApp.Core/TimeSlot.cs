using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LSC.RestaurantTableBookingApp.Core;

public partial class TimeSlot
{
    public int Id { get; set; }

    [Required]
    public int BranchId { get; set; }

    [Required]
    public DateTime ReservationDay { get; set; }
    [Required]
    public string MealType { get; set; } = null!;
    [Required]
    public string TableStatus { get; set; } = null!;

    public virtual RestaurantBranch Branch { get; set; } = null!;

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
