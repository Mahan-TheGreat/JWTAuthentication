using JWTAuthentication.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseInMemoryDatabase("Authentication");
});

builder.Services.AddScoped<IApplicationDBContext, ApplicationDBContext>();
builder.Services.AddScoped<ApplicationDBContextInitializer>();
var app = builder.Build();

var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<ApplicationDBContextInitializer>().SeedDataAsync();
scope.Dispose();

app.UseCors(builder=>
{
    builder.WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithExposedHeaders("X-Pagination");
});

// Configure the HTTP request pipeline.a
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }