using JWTAuthentication.Entities;
using JWTAuthentication.Models;

namespace JWTAuthentication.Helper;

public interface ITokenHelper
{
    string CreateJwtToken(User user);
    RefreshToken GenerateRefreshToken();
    string CreateRandomToken();
}
