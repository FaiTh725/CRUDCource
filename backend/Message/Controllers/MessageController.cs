using Message.Domain.Contracts.Modals.Message;
using Message.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Message.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService messageService;

        public MessageController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpGet("[action]")]
        //[Authorize]
        public async Task<IActionResult> GetChatHistory(string chatId)
        {
            var response = await messageService.GetChatMessages(chatId);

            return new JsonResult(response);
        }

    }
}
