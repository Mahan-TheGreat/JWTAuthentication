using System.Security.Claims;

namespace JWTAuthentication.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int GetUserId()
        {
            int id = -1;
            if(_httpContextAccessor != null && _httpContextAccessor.HttpContext != null )
            {
                var UserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                id = int.Parse(UserId);
            }
            return id;
        }

        public string GetUserName()
        {
            string Username = string.Empty;
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                Username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
            return Username;
        }

        public string GetUserRole()
        {
            string UserRole = string.Empty;
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                UserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            }
            return UserRole;
        }
    }
}
