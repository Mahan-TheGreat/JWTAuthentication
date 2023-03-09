using JWTAuthentication.Entities;
using JWTAuthentication.Helper;
using JWTAuthentication.Infrastructure;
using JWTAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace JWTAuthentication.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    private ApplicationDBContext _Context;
    private IConfiguration _configuration;
    private TokenHelper _tokenHelper;


    public UserController(
        ApplicationDBContext Context,
        IConfiguration configuration
        )
    {
       
        _Context = Context;
        _configuration = configuration;
       _tokenHelper = new TokenHelper(_configuration);

}

[HttpGet]
    public async Task<List<User>> GetUsers()
    {
        return await _Context.Users.ToListAsync();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDTO user)
    {
        if(_Context.Users.Any(u=> u.Email == user.Email || u.UserName == user.UserName))
        {
            return BadRequest("User Already Exists.");
        }
        CreatePasswordHash(user.Password,
            out byte[] passwordHash,
            out byte[] passwordSalt);
        var registerUser = new User
        {
            Id = 0,
            UserName = user.UserName,
            Email = user.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            UserRole = user.UserRole,
            VerificationToken = _tokenHelper.CreateRandomToken()
        };

        _Context.Users.Add(registerUser);
        await _Context.SaveChangesAsync();
        await VerifyUser(registerUser.VerificationToken);

        return Ok(new
        {
            Message = "User registered successfully."
        });
     
    }


    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserLoginDTO request)
    {

        var user = await _Context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
        if (user == null)
        {
            return BadRequest("User not Found!");
        }
        if (!VerifyPasswordHash(request.Password,user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest("Password do not match.");
        }
        string token = _tokenHelper.CreateJwtToken(user);

        var refreshToken = _tokenHelper.GenerateRefreshToken();
        SetRefreshToken(user, refreshToken);
        SetUserSession(user);
        return Ok(token);

    }

    private async void SetUserSession(User user)
    {
        LoggedInUser User = new LoggedInUser();

        User.SessionId = _tokenHelper.CreateRandomToken();
        User.UserId = user.Id;
        User.SessionStart = DateTime.Now;

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
        };

        await _Context.SessionHistory.AddAsync(User);
        await _Context.SaveChangesAsync();

        Response.Cookies.Append("sessionId", User.SessionId.ToString(), cookieOptions);

    }

    [HttpGet("Sessionhistory"), Authorize(Roles = "Admin")]
    private async Task<List<LoggedInUser>> GetUserSession()
    {
        var sessions = await _Context.SessionHistory.ToListAsync();
        return sessions;
    }

    [HttpPost("Logout"), Authorize]
    public  OkObjectResult Logout(int id)
    {
             EndUserSession();
             RemoveRefreshToken(id);
            return Ok(new
            {
             Message = "User logged out."
            });
    }


    private async void EndUserSession()
    {
        if (Request.Cookies["sessionId"] != null)
        {
            string sessionId = Request.Cookies["sessionId"]!;
            var session = await _Context.SessionHistory.FirstAsync(x => x.SessionId == sessionId);
            session.SessionEnd = DateTime.Now;
            await _Context.SaveChangesAsync();
        }
    
    }
    private async Task<IActionResult> VerifyUser(string token)
    {

        var user = await _Context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
        if (user == null)
        {
            return BadRequest("Invalid Token!");
        }
        user.VerifiedAt = DateTime.Now;
        await _Context.SaveChangesAsync();

        return Ok(new
        {
            Message = "User Verified."
        });

    }

    [HttpPost("forgotPassword")]
    public async Task<IActionResult> ForgotPassword(string email)
    {

        var user = await _Context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return BadRequest("User not found!");
        }
        user.PasswordResetToken = _tokenHelper.CreateRandomToken();
        user.ResetTokenExpires = DateTime.Now.AddHours(1);
        await _Context.SaveChangesAsync();

        return Ok(new
        {
            Message = "You can now reset your password."
        });

    }

    [HttpPost("resetPassword")]
    public async Task<IActionResult> ForgotPassword(ResetPasswordDTO request)
    {

        var user = await _Context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
        {
            return BadRequest("User not found!");
        }
        if (user.ResetTokenExpires < DateTime.Now || user.PasswordResetToken != request.Token)
        {
            return BadRequest("Token is invalid!");
        }
        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        user.PasswordSalt= passwordSalt;
        user.PasswordHash = passwordHash;
        user.PasswordResetToken = null;
        user.ResetTokenExpires = null;

        await _Context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Password reset successfull."
        });

    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using(var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }



    [HttpPost("RefreshToken"), Authorize]

    public async Task<ActionResult<string>> RefreshToken(int userId)
    {
        var user = await _Context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return BadRequest("Invalid User!");
        }
        var refreshTOken = Request.Cookies["refreshToken"];
        if (!user.RefreshToken!.Equals(refreshTOken))
        {
            return Unauthorized("Invalid Refresh Token!");
        }
        else if(user.RefreshTokenExpires < DateTime.Now)
        {
            return Unauthorized("Token has expired.");
        }

        string token = _tokenHelper.CreateJwtToken(user);
        var newRefreshToken = _tokenHelper.GenerateRefreshToken();
        SetRefreshToken(user, newRefreshToken);

        return Ok(token);
    }


    private async void SetRefreshToken(User user,RefreshToken refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = refreshToken.ExpiresAt
        };
        Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        user.RefreshToken = refreshToken.Token;
        user.RefreshTokenCreated = refreshToken.CreatedAt;
        user.RefreshTokenExpires = refreshToken.ExpiresAt;
        await _Context.SaveChangesAsync();
    }

    private async void RemoveRefreshToken(int userId)
    {
        User user = await _Context.Users.FirstAsync(u => u.Id == userId);
        user.RefreshToken = null;
        user.RefreshTokenCreated = null;
        user.RefreshTokenExpires = null;
        Response.Cookies.Delete("refreshToken");
        await _Context.SaveChangesAsync();
    }

}
