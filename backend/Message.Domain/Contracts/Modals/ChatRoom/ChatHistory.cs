using MessageEntity = Message.Domain.Entities.Message;

namespace Message.Domain.Contracts.Modals.ChatRoom
{
    public class ChatHistory
    {
        public string ChatId { get; set; } = string.Empty;

        public List<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
    }
}
