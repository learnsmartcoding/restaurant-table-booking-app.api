using System.ComponentModel.DataAnnotations;

namespace LSC.RestaurantTableBookingApp.Core;

public partial class DiningTable
{
    public int Id { get; set; }
        
    public int RestaurantBranchId { get; set; }

    
    [MaxLength(100)]
    public string? SeatsName { get; set; }

    [Required]
    public int Capacity { get; set; }

    public virtual RestaurantBranch Branch { get; set; } = null!;
    
}
