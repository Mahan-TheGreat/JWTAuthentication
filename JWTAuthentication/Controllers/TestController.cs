using JWTAuthentication.Infrastructure;
using JWTAuthentication.Models;
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
    }
}
