using JWTAuthentication.Entities;
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

}
