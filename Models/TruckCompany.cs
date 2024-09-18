using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class TruckCompany
{
    [Key]
    public int CompanyId { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Name { get; set; }
    
    [StringLength(255)]
    public string Address { get; set; }
    
    [StringLength(20)]
    public string ContactNumber { get; set; }
    
    public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;
    
    [StringLength(255)]
    public string Email { get; set; }
    
    [StringLength(255)]
    public string Website { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Add these properties
    public ICollection<Driver> Drivers { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<Container> Containers { get; set; }
}
