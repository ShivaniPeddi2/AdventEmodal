// Models/Driver.cs
using System;
using System.ComponentModel.DataAnnotations;
public class Driver
{
    [Key]
    public int DriverId { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Name { get; set; }
    
    [Required]
    [StringLength(50)]
    public string LicenseNumber { get; set; }
    
    [StringLength(20)]
    public string PhoneNumber { get; set; }
    
    public int? TruckCompanyId { get; set; }
    public TruckCompany TruckCompany { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
