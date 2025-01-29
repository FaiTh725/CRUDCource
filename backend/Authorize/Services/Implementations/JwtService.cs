using Application.Contracts.SharedModels.Exceptions;
using Authorize.Domain.Entities;
using Authorize.Domain.Modals.Auth;
using Authorize.Helpers.Jwt;
using Authorize.Services.Interfaces;
using CSharpFunctionalExtensions;
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

        public Result<TokenData> DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var name = jwtSecurityToken.Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
            var email = jwtSecurityToken.Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
            var role = jwtSecurityToken.Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;

            if (name == null || 
                email == null || 
                role == null)
            {
                return Result.Failure<TokenData>("Invalid Token");
            }

            return Result.Success(new TokenData
            {
                Name = name,
                Email = email,
                Role = role
            });
        }


        public string GenerateToken(string userName, User user)
        {
            var jwtSetting = configuration
                .GetSection("JwtSetting")
                .Get<JwtSetting>();

            if(jwtSetting is null)
            {
                throw new AppConfigurationException("JWtSetting");
            }

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, userName),
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

        public string GenerateToken(User user)
        {
            var jwtSetting = configuration
                .GetSection("JwtSetting")
                .Get<JwtSetting>();

            if (jwtSetting is null)
            {
                throw new AppConfigurationException("JWtSetting");
            }

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey)),
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
