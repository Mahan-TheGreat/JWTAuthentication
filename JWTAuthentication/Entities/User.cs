namespace JWTAuthentication.Entities;

public class User:BaseEntity
{
    public string UserName { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = new byte[32];
    public byte[] PasswordSalt { get; set; } = new byte[32];
    public string Email { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;
    public string? VerificationToken { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? PasswordResetToken { get; set;}
    public DateTime? ResetTokenExpires { get; set; }

    public string? RefreshToken { get; set; } = string.Empty;

    public DateTime? RefreshTokenCreated { get; set;}

    public DateTime? RefreshTokenExpires { get; set; }

}
