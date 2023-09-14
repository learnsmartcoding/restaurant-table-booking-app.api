using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LSC.RestaurantTableBookingApp.Core;

public partial class DiningTable
{
    public int Id { get; set; }
    
    public int RestaurantBranchId { get; set; }

    
    [MaxLength(100)]
    public string? TableName { get; set; }

    [Required]
    public int Capacity { get; set; }

    public virtual RestaurantBranch Branch { get; set; } = null!;
    public virtual ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
   
}

//public partial class Restaurant
//{
//    //    [Key] Id is automatically considered as primary key by EF, if name is different then specify as [Key]
//    public int Id { get; set; }

//    [Required]
//    [MaxLength(100)]
//    public string Name { get; set; } = null!;

//    [Required]
//    [MaxLength(200)]
//    public string Address { get; set; } = null!;


//    [MaxLength(20)]
//    public string? Phone { get; set; }


//    [MaxLength(100)]
//    public string? Email { get; set; }


//    [MaxLength(500)]
//    public string? ImageUrl { get; set; }

//    public virtual ICollection<RestaurantBranch> RestaurantBranches { get; set; } = new List<RestaurantBranch>();
//}

//public partial class Reservation
//{
//    public int Id { get; set; }

//    public int UserId { get; set; }

//    public int TimeSlotId { get; set; }

//    public DateTime ReservationDate { get; set; }

//    public string ReservationStatus { get; set; } = null!;

//    public virtual TimeSlot TimeSlot { get; set; } = null!;

//    public virtual User User { get; set; } = null!;
//}
//public partial class RestaurantBranch
//{
//    public int Id { get; set; }

//    public int RestaurantId { get; set; }

//    [Required]
//    [MaxLength(100)]
//    public string Name { get; set; } = null!;

//    [Required]
//    [MaxLength(200)]
//    public string Address { get; set; } = null!;


//    [MaxLength(20)]
//    public string? Phone { get; set; }


//    [MaxLength(100)]
//    public string? Email { get; set; }


//    [MaxLength(500)]
//    public string? ImageUrl { get; set; }

//    public virtual ICollection<DiningTable> DiningTables { get; set; } = new List<DiningTable>();

//    public virtual Restaurant Restaurant { get; set; } = null!;


//}
//public partial class TimeSlot
//{
//    public int Id { get; set; }
//    public int DiningTableId { get; set; }

//    [Required]
//    public DateTime ReservationDay { get; set; }
//    [Required]
//    public string MealType { get; set; } = null!;
//    [Required]
//    public string TableStatus { get; set; } = null!;
//    public virtual DiningTable DiningTable { get; set; } = null!;

//    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
//}
//public partial class User
//{
//    public int Id { get; set; }

//    [Required]
//    [MaxLength(50)]
//    public string FirstName { get; set; } = null!;
//    [Required]
//    [MaxLength(50)]
//    public string LastName { get; set; } = null!;
//    [Required]
//    [MaxLength(100)]
//    public string Email { get; set; } = null!;

//    [MaxLength(128)]
//    public string? AdObjId { get; set; }
//    [MaxLength(512)]
//    public string? ProfileImageUrl { get; set; }
//    public DateTime CreatedDate { get; set; }
//    public DateTime UpdatedDate { get; set; }

//    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
//}
