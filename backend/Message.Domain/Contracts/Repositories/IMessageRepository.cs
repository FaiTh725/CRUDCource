using CSharpFunctionalExtensions;
using Message.Domain.Contracts.Modals.ChatRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
