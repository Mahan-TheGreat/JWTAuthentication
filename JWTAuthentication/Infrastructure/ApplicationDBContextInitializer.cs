using JWTAuthentication.Entities;

namespace JWTAuthentication.Infrastructure;

public class ApplicationDBContextInitializer
{
    private readonly IApplicationDBContext _context;

	public ApplicationDBContextInitializer(IApplicationDBContext context)
	{
		_context = context;
	}

	public async Task SeedDataAsync()
	{
		await SeedUsersData();
	}

	public async Task SeedUsersData()
	{
		var user = new User()
		{
			UserName = "Admin",
			Email = "admin@danphecare.com",
			Password = "Password@123"

		};

		_context.Users.Add(user);
		await _context.SaveChangesAsync();
	}
}
