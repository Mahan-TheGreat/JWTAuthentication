using JWTAuthentication.Entities;
using JWTAuthentication.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.Infrastructure;

public class ApplicationDBContext: DbContext, IApplicationDBContext
{
    //protected override void OnConfiguring
    //    (DbContextOptionsBuilder optionBuilder)
    //{
    //    optionBuilder.UseInMemoryDatabase(databaseName: "AuthenticationDb");
    //}
    public ApplicationDBContext(DbContextOptions options) : base(options)
    { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Superhero> Superheroes => Set<Superhero>();

    public DbSet<LoggedInUser> SessionHistory => Set<LoggedInUser>();

}
