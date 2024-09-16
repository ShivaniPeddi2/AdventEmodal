// Models/Container.cs
using System;
using System.ComponentModel.DataAnnotations;

public class Container
{
    [Key]
    public int ContainerId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string ContainerNumber { get; set; }
    
    [Required]
    [StringLength(50)]
    public string ChassisNumber { get; set; }
    
    [Required]
    [StringLength(50)]
    public string ContainerType { get; set; }
    
    [Required]
    public int Capacity { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Status { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
