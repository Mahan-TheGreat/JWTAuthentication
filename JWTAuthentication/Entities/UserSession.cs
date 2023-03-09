namespace JWTAuthentication.Entities;

public class UserSession : BaseEntity
{
    public string SessionId { get; set; } = string.Empty;

    public int UserId { get; set; }

    public DateTime SessionStart { get; set; }

    public DateTime? SessionEnd { get; set; }

}
