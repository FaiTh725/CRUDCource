using Authorize.Domain.Entities;
using Authorize.Domain.Modals.Auth;
using CSharpFunctionalExtensions;

namespace Authorize.Services.Interfaces
{
    public interface IJwtService
    {
        public string GenerateToken(string userName, User user);
        public string GenerateToken(User user);

        public Result<TokenData> DecodeToken(string token);
    }
}
