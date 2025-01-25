using CSharpFunctionalExtensions;
using MessageEntity = Message.Domain.Entities.Message;

namespace Message.Domain.Contracts.Repositories
{
    public interface IMessageRepository
    {
        Task<Result> AddMessage(MessageEntity messageEntity);

        Task<List<MessageEntity>> GetChatMessages(string chatId);

        Task<bool> DeleteChatHistory(string chatId);
    }
}
