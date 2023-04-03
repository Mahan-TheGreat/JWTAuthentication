using JWTAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace JWTAuthentication.Controllers;

[ApiController]
[Route("api/[controller]")]

public class  GoogleAuthenticationController: ControllerBase
{
    [HttpPost]
    public Dictionary<string, string> HandleGoogleCredential(GoogleCredentialDTO cred)
    {
        var tokenInfo = GetTokenInfo(cred.Credential);
        return tokenInfo;
    }


    protected Dictionary<string, string> GetTokenInfo(string token)
        {
            var TokenInfo = new Dictionary<string, string>();

        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var claims = jwtSecurityToken.Claims.ToList();

        foreach (var claim in claims)
        {

            TokenInfo.Add(claim.Type, claim.Value);
        }

        return TokenInfo;
    }
}