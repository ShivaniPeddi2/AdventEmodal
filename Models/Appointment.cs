using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Appointment
{
    [Key]
    public int AppointmentId { get; set; }
    
    public int? UserId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
    
    public int? ContainerId { get; set; }
    [JsonIgnore]
    public Container? Container { get; set; }
    
    public int? TerminalId { get; set; }
    [JsonIgnore]
    public Terminal? Terminal { get; set; }
    
    public int? DriverId { get; set; }
    [JsonIgnore]
    public Driver? Driver { get; set; }
    
    // Add this property
    public int? CompanyId { get; set; }
    [JsonIgnore]
    public TruckCompany? TruckCompany { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Status { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal TotalCost { get; set; }
    
    [Required]
    [StringLength(50)]
    public string TicketNumber { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [StringLength(50)]
    public string MoveType { get; set; }
    
    [StringLength(50)]
    public string GateCode { get; set; }
    
    [StringLength(50)]
    public string AppointmentStatus { get; set; }
    
    [StringLength(50)]
    public string GateStatus { get; set; }
    
    //[StringLength(50)]
    //[JsonIgnore]
    //public int Line { get; set; }
    
    //[JsonIgnore]
    public DateTime? CheckIn { get; set; }
    
    [StringLength(50)]
    public string TransportType { get; set; }
}
