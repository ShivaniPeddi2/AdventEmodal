using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    public string? PhoneNumber { get; set; }
    
    public int? TruckCompanyId { get; set; }
    
    [JsonIgnore]
    public TruckCompany? TruckCompany { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public ICollection<Appointment>? Appointments { get; set; }
}
