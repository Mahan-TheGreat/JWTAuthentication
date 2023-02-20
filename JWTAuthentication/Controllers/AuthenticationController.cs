using JWTAuthentication.Entities;
using JWTAuthentication.Infrastructure;
using JWTAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace JWTAuthentication.Controllers;

public class AuthenticationController : ControllerBase
{
    public ApplicationDBContext _Context;

    public AuthenticationController(ApplicationDBContext Context)
    {
        _Context = Context;
    }

    [HttpGet]
    public async Task<List<User>> GetUsers()
    {
        return await _Context.Users.ToListAsync();
    }

    [HttpPost]
    public IActionResult RegisterUsers(UserRegisterDTO entity)
        {
        var user = new UserDTO
        {
            UserName = entity.UserName,
            PasswordHash = this.encryptPassword(entity.Password,), 
            PassWordSalt = entity.PasswordSalt
        };
            _Context.Users.Add(entity);
            _Context.SaveChangesAsync();

            return entity.Id;
        }

    encryptPassword = 

}
