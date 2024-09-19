using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class AdminModel
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
    public string Username { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; }

    [EmailAddress]
    [Required]
    public string Email { get; set; }

    // You may add additional properties here if needed, e.g., Role or Permissions
    public string Role { get; set; }
}
