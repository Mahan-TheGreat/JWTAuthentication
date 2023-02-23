using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Models;

public class UserRegisterDTO
{
    [Required(ErrorMessage = "First Name is required")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last Name is required")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "User Name is required")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required"), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required"), MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm Password is required"), Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required")]
    public string UserRole { get; set; } = string.Empty;

}
