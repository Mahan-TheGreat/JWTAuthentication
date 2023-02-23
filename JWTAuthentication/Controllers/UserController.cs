﻿using JWTAuthentication.Entities;
using JWTAuthentication.Infrastructure;
using JWTAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JWTAuthentication.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    private ApplicationDBContext _Context;
    private IConfiguration _configuration;


    public UserController(ApplicationDBContext Context, IConfiguration configuration)
    {

        _Context = Context;
        _configuration = configuration;
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
            VerificationToken = CreateRandomToken()
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
        string token = CreateJwtToken(user);

        var refreshToken = GenerateRefreshToken();
        SetRefreshToken(user, refreshToken);
        return Ok(token);

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
        user.PasswordResetToken = CreateRandomToken();
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

    private string CreateJwtToken(User user)
    {

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                 new Claim(ClaimTypes.Name, user.UserName),
                 new Claim(ClaimTypes.Role, user.UserRole),
                 new Claim("aud","http://localhost:7130"),
                 new Claim("aud","http://localhost:4200"),
                 
            }),
            Issuer = "https://localhost:7130",
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
    };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var jwt = tokenHandler.WriteToken(token);

        return jwt;

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

        string token = CreateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();
        SetRefreshToken(user, newRefreshToken);

        return Ok(token);
    }

    private RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            CreatedAt = DateTime.Now
        };
        return refreshToken;
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

    private string CreateRandomToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    }

}
