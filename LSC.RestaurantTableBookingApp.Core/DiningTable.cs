using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LSC.RestaurantTableBookingApp.Core;

public partial class DiningTable
{
    public int Id { get; set; }

    [Required]
    public int BranchId { get; set; }

    
    [MaxLength(100)]
    public string? SeatsName { get; set; }

    [Required]
    public int Capacity { get; set; }

    public virtual RestaurantBranch Branch { get; set; } = null!;

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
