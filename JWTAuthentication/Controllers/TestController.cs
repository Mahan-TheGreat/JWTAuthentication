using JWTAuthentication.Entities;
using JWTAuthentication.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private ApplicationDBContext _context;
        public TestController(ApplicationDBContext context)
        {
            _context= context;
        }
        
        [HttpGet("Superheroes"), Authorize]
        public async Task<List<Superhero>> GetSuperheroes()
        {
            return await _context.Superheroes.ToListAsync();
        }

        [HttpPost("Superheroes"), Authorize(Roles = "Admin")]

        public async Task<IActionResult> PostSuperheroes(Superhero hero)
        {
            _context.Superheroes.Add(hero);
            await _context.SaveChangesAsync();
            return Ok(hero);
        }
    }
}
