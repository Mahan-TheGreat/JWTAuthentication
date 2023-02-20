namespace JWTAuthentication.Models;

public class UserDTO
{
    public string UserName { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = new byte[200];  
    public byte[] PasswordSalt { get; set; } = new byte[200];
    public string Email { get; set; } = string.Empty;
}
