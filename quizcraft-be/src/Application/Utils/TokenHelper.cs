using Microsoft.IdentityModel.Tokens;
using src.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace src.Application.Utils
{
    public static class TokenHelper
    {
        public static string GenerateJwtToken(string email,Role role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("KEY")!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(

                issuer: Environment.GetEnvironmentVariable("ISSUER"),
                audience: Environment.GetEnvironmentVariable("AUDIENCE"),
                claims: GenerateClaims(email,role),
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials

                );

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(tokenDescriptor);
        }

        private static Claim[] GenerateClaims(string email, Role role)
        {
            return new[] {
                             new Claim(ClaimTypes.Role, role.ToString()),
                             new Claim(ClaimTypes.Email, email)
                         };
        }
    }
}