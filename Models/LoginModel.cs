using System.ComponentModel.DataAnnotations;

public class LoginModel
{
    [Required]
    [StringLength(255)]
    public string Username { get; set; }

    [Required]
    [StringLength(255)]
    public string Password { get; set; }
}
