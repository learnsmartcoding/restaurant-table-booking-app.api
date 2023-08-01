using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LSC.RestaurantTableBookingApp.Core;

public partial class TimeSlot
{
    public int Id { get; set; }
    public int DiningTableId { get; set; }

    [Required]
    public DateTime ReservationDay { get; set; }
    [Required]
    public string MealType { get; set; } = null!;
    [Required]
    public string TableStatus { get; set; } = null!;    
    public virtual DiningTable DiningTable { get; set; } = null!;

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
