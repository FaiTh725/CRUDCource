using Microsoft.AspNetCore.Mvc;
using Notification.Domain.Contacrs.Email;
using Notification.Services.Interfaces;

namespace Notification.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailSendlerController : ControllerBase
    {
        private readonly ISendEmailService emailService;

        public EmailSendlerController(ISendEmailService sendEmailService)
        {
            emailService = sendEmailService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendEmail(SendEmail request)
        {
            var result = await emailService.SendEmail(request);

            return new JsonResult(result);
        }
    }
}
