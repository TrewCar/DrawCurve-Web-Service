using DrawCurve.Domen.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DrawCurve.API.Menedgers
{
    public class JwtManager
    {
        private readonly IConfiguration _configuration;

        public JwtManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public User? GetUserSession(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out var securityToken);

                var jwtToken = (JwtSecurityToken)securityToken;
                var userId = principal.FindFirst(ClaimTypes.Name)?.Value;
                var userRole = principal.FindFirst(ClaimTypes.Role)?.Value;

                if (userId != null && userRole != null)
                {
                    return new User
                    {
                        Id = int.Parse(userId),
                        Role = Enum.Parse<Role>(userRole),
                        Name = principal.Identity.Name // or retrieve name from claims if available
                    };
                }
            }
            catch
            {
                // Handle token validation failure
            }

            return null;
        }
    }
}
