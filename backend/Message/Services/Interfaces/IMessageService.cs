using Application.Contracts.Response;
using Message.Domain.Contracts.Modals.Message;

namespace Message.Services.Interfaces
{
    public interface IMessageService
    {
        Task<DataResponse<List<MessageResponse>>> GetChatMessages(string chatId);
    }
}
