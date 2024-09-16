// Models/Terminal.cs

using System;
using System.ComponentModel.DataAnnotations;

public class Terminal
{
    [Key]
    public int TerminalId { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Address { get; set; }
    
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
