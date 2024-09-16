using System;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [StringLength(255)]
    public string Username { get; set; }

    [Required]
    [StringLength(255)]
    public string Password { get; set; } // Store plain text password

    [Required]
    [StringLength(255)]
    public string Email { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsAdmin { get; set; } // New property for admin role
}
