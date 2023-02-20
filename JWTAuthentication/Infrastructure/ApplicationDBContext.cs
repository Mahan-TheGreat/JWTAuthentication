﻿using JWTAuthentication.Entities;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.Infrastructure;

public class ApplicationDBContext: DbContext
{
    protected override void OnConfiguring
        (DbContextOptionsBuilder optionBuilder)
    {
        optionBuilder.UseInMemoryDatabase(databaseName: "AuthenticationDb");
    }

    public DbSet<User> Users => Set <User>();

}
