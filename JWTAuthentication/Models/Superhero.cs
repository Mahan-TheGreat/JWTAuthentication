
using JWTAuthentication.Entities;

namespace JWTAuthentication.Models;


public class Superhero: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
