using JWTAuthentication.Models;

namespace JWTAuthentication.Infrastructure;

public class ApplicationDBInitializer
{
    private readonly IApplicationDBContext _context;

	public ApplicationDBInitializer(IApplicationDBContext context)
	{
		_context= context;
	}

	public async Task SeedDataAsync()
	{
		await SeedSuperheroesData();
	}

	private async Task SeedSuperheroesData()
	{
		var Superheroes = new List<Superhero>()
		{
			new Superhero()
			{
				Id = 1,
				Name = "Superman",
				IsActive = true
			},
            new Superhero()
            {
                Id = 2,
                Name = "Batman",
                IsActive = true
            },
            new Superhero()
            {
                Id = 3,
                Name = "Spiderman",
                IsActive = true
            },
        };
		_context.Superheroes.AddRange(Superheroes);
		await _context.SaveChangesAsync();
	}
}
