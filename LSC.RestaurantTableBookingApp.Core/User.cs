using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LSC.RestaurantTableBookingApp.Core;

public partial class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = null!;

    [MaxLength(128)]
    public string? AdObjId { get; set; }
    [MaxLength(512)]
    public string? ProfileImageUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
