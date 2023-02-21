using JWTAuthentication.Entities;
using JWTAuthentication.Infrastructure;
using JWTAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;

namespace JWTAuthentication.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    private ApplicationDBContext _Context;


    public UserController(ApplicationDBContext Context)
    {

        _Context = Context;
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
            VerificationToken = CreateToken()

        };

        _Context.Users.Add(registerUser);
        await _Context.SaveChangesAsync();

        return Ok(new
        {
            Message = "User registered successfully."
        });
     
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDTO request)
    {

        var user = await _Context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
        if (user == null)
        {
            return BadRequest("User not Found!");
        }
        if (user.VerifiedAt == null )
        {
            return BadRequest("User not Verified!");
        }
        if (!VerifyPasswordHash(request.Password,user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest("Password do not match.");
        }
        return Ok(new
        {
            Message = "User login successfull."
        }); 

    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify(string token)
    {

        var user = await _Context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
        if (user == null)
        {
            return BadRequest("Invalid Token!");
        }
        user.VerifiedAt= DateTime.Now;
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
        user.PasswordResetToken = CreateToken();
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

    private string CreateToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    }

}
