using Application.Contracts.Response;
using MailKit.Net.Smtp;
using MimeKit;
using Notification.Domain.Contacrs.Email;
using Notification.Helpers.Setting;
using Notification.Services.Interfaces;

namespace Notification.Services.Implementations
{
    public class SendEmailService : ISendEmailService
    {
        private readonly IConfiguration configuration;

        public SendEmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<BaseResponse> SendEmail(SendEmail emailData)
        {
            try
            {
                var emailSetting = configuration.GetSection("EmailSMTPSetting").Get<EmailSetting>();

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Moi Sled Zelenukho )))", emailSetting!.Email));
                foreach (var recipientAdress in emailData.Recipients)
                {
                    message.To.Add(new MailboxAddress("", recipientAdress));
                }
                message.Subject = emailData.Subject;
                message.Body = new TextPart("plain")
                {
                    Text = emailData.Body
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(emailSetting.Host, emailSetting.Port);
                await client.AuthenticateAsync(emailSetting.Email, emailSetting.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return new BaseResponse
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Successfull send email"
                };
            }
            catch
            {
                return new BaseResponse
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Error with send email"
                };
            }
        }
    }
}
