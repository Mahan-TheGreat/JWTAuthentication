using JWTAuthentication.Entities;
using JWTAuthentication.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    public ApplicationDBContext _Context;

    public UserController(ApplicationDBContext Context)
    {
        _Context = Context;
    }

    [HttpGet]
    public async Task<List<User>> GetUsers()
    {
        return await _Context.Users.ToListAsync();
    }

    //[HttpPost]
    //public IActionResult RegisterUsers(UserRegisterDTO entity)
    //    {
    //    var user = new UserDTO
    //    {
    //        UserName = entity.UserName,
    //        PasswordHash = this.encryptPassword(entity.Password,), 
    //        PassWordSalt = entity.PasswordSalt
    //    };
    //        _Context.Users.Add(entity);
    //        _Context.SaveChangesAsync();

    //        return entity.Id;
    //    }

    //encryptPassword = 

}
