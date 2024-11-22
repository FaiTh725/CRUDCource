using Authorize.Domain.Entities;

namespace Authorize.Services.Interfaces
{
    public interface IJwtService
    {
        public string GenerateToken(User user);
    }
}
