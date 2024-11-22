using Authorize.Domain.Entities;
using Authorize.Helpers.Jwt;
using Authorize.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authorize.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration configuration;

        public JwtService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var jwtSetting = configuration.GetSection("JwtSetting").Get<JwtSetting>();

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting!.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Role)
            };

            var token = new JwtSecurityToken
            (
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(3),
                audience: jwtSetting.Audience,
                issuer: jwtSetting.Issuer
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
