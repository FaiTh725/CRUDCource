using Authorize.Domain.Entities;

namespace Authorize.Services.Interfaces
{
    public interface IJwtService
    {
        public string GenerateToken(string userName, User user);
        public string GenerateToken(User user);
    }
}
