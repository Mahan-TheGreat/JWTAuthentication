namespace JWTAuthentication.Services.UserService;

public interface IUserService
{
    int GetUserId();
    string GetUserName();
    string GetUserRole();

}
