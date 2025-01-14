using Message.Domain.Contracts.Modals.ChatRoom;
using Message.Domain.Contracts.Modals.Message;

namespace Message.Hubs.Interfaces
{
    public interface IMessageHub
    {
        Task ChatCreatedResult(ChatRoomResponse chatRoom);

        Task UserConnected(List<ChatRoomResponse> chats);

        Task SellerConnected(List<ChatRoomResponse> chats);

        Task ChatDeleted(string chatId);

        Task MessageSent(MessageResponse message);

        //Task ChatOpened(List<MessageResponse> messages);
    }
}
