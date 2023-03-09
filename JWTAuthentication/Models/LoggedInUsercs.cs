using JWTAuthentication.Entities;

namespace JWTAuthentication.Models;

public class LoggedInUser: BaseEntity
{
    public string SessionId { get; set; } = string.Empty;

    public int UserId { get; set; }

    public DateTime SessionStart { get; set; }

    public DateTime? SessionEnd { get; set; }
    
}
