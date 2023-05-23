using System.ComponentModel.DataAnnotations;

namespace Blazelounge_v2.Data.Models;

public class RegisterModel
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username must not contain special characters")]
    public string Username { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string First_Name { get; set; }

    [Required]
    public string Last_Name { get; set; }
}
