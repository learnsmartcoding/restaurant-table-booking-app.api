using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LSC.RestaurantTableBookingApp.Core;

public partial class RestaurantBranch
{
    public int Id { get; set; }

    public int RestaurantId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = null!;

    
    [MaxLength(20)]
    public string? Phone { get; set; }

    
    [MaxLength(100)]
    public string? Email { get; set; }

    
    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public virtual ICollection<DiningTable> DiningTables { get; set; } = new List<DiningTable>();

    public virtual Restaurant Restaurant { get; set; } = null!;

   
}
