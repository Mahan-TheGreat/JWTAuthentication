using JWTAuthentication.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserClaimsController : ControllerBase
{
    private readonly IUserService _userService;

    public UserClaimsController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("UserId"), Authorize]
    public int GetUserId()
    {
        return _userService.GetUserId();
    }

    [HttpGet("UserName"), Authorize]
    public string GetUserName()
    {
        return _userService.GetUserName();
    }

    [HttpGet("UserRole"), Authorize]
    public string GetUserRole()
    {
        return _userService.GetUserRole();
    }
}
