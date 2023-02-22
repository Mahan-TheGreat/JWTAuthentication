using JWTAuthentication.Entities;
using JWTAuthentication.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.Infrastructure;

public interface IApplicationDBContext
{
    DbSet<User> Users { get; }
    DbSet<Superhero> Superheroes { get; }


    Task<int> SaveChangesAsync(CancellationToken canToken = default);
}
