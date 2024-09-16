// Models/TruckCompany.cs
using System;
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
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}