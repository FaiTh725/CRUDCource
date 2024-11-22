using MassTransit;
using Notification.Domain.Contacrs.Email;
using Notification.Services.Interfaces;

namespace Notification.Features.Consumers
{
    public sealed class SentEmailConsumer : IConsumer<SendEmail>
    {
        private readonly ISendEmailService emailService;

        public SentEmailConsumer(ISendEmailService emailService)
        {
            this.emailService = emailService;
        }

        public async Task Consume(ConsumeContext<SendEmail> context)
        {
            await emailService.SendEmail(context.Message);
        }
    }
}
