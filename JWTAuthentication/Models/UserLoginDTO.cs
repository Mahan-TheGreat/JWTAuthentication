using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Models;

public class UserLoginDTO
{
    [Required(ErrorMessage = "User Name is required")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

}
