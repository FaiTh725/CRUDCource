using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Message.Hubs.Providers
{
    public class EmailUserProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
