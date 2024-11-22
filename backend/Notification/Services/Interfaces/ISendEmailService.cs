using Application.Contracts.Response;
using Notification.Domain.Contacrs.Email;

namespace Notification.Services.Interfaces
{
    public interface ISendEmailService
    {
        public Task<BaseResponse> SendEmail(SendEmail emailData);
    }
}
