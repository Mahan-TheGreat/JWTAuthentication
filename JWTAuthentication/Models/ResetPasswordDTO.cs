using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Models;

public class ResetPasswordDTO
{
    [Required(ErrorMessage = "Email is required"), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required"), MinLength(6, ErrorMessage = "Password must be atlease 6 characters long!")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm Password is required"), Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Token is required")]
    public string? Token { get; set; }

}
