
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public static class GenerateJwtToken
{
    public static string Generate(string user)
    {

        var claims = new[]
        {
             new Claim(ClaimTypes.NameIdentifier, user)
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("fajŞASJKFAJFASKABGJASGJABGJAGBKJSABGABJGASJKGSAKBJBJKGJBKASGS"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "localhost:5000",
            audience: "localhost:3000",
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}