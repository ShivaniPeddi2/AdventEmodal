using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

    [Required]
    public int CompanyId { get; set; }

    [ForeignKey(nameof(CompanyId))]
    [JsonIgnore]
    public TruckCompany? TruckCompany { get; set; }
}
