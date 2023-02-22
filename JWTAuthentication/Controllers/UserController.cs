using JWTAuthentication.Entities;
using JWTAuthentication.Infrastructure;
using JWTAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        //List<Claim> claims = new List<Claim>
        //{
        //    new Claim(ClaimTypes.Name, user.UserName),
        //    new Claim("aud", "https://localhost:7130")
        //};

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.Name, user.UserName),
                 new Claim("aud","http://localhost:7130"),
                 new Claim("aud","http://localhost:4200"),
                 
            }),
            Issuer = "https://localhost:7130",
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
    };

        //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        //var token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(1),
        //        signingCredentials: creds
        //    );
        var jwt = tokenHandler.WriteToken(token);

        return jwt;

    }

    private string CreateRandomToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    }

}
