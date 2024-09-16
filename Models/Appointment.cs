// Models/Appointment.cs
using System;
using System.ComponentModel.DataAnnotations;
public class Appointment
{
    [Key]
    public int AppointmentId { get; set; }
    
    public int? UserId { get; set; }
    public User User { get; set; }
    
    public int? ContainerId { get; set; }
    public Container Container { get; set; }
    
    public int? TerminalId { get; set; }
    public Terminal Terminal { get; set; }
    
    public int? DriverId { get; set; }
    public Driver Driver { get; set; }
    
    public int? TruckCompanyId { get; set; }
    public TruckCompany TruckCompany { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
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
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}