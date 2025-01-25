using Message.Domain.Contracts.Modals.Message;
namespace Message.Domain.Contracts.Modals.ChatRoom
{
    public class ChatRoomResponse
    {
        public string Id { get; set; } = string.Empty;

        public string CustomerEmail { get; set; } = string.Empty;

        public string SellerEmail {  get; set; } = string.Empty ;

        public long ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public List<MessageResponse> Messages { get; set; } = new List<MessageResponse>();
    }
}
