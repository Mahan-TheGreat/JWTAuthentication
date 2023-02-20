using JWTAuthentication.Entities;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.Infrastructure;

public interface IApplicationDBContext
{
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken canToken = default);
}
