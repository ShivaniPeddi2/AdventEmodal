using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Terminal
{
    [Key]
    public int TerminalId { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Address { get; set; }
    
    [StringLength(100)]
    public string State { get; set; }
    
    [StringLength(20)]
    public string Pincode { get; set; }
    
    [StringLength(100)]
    public string Country { get; set; }
    
    [StringLength(50)]
    public string GateNo { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    // Add this property
    public ICollection<Appointment>? Appointments { get; set; }
}
