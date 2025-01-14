using Application.Contracts.Response;
using Message.Domain.Contracts.Modals.Message;
using Message.Services.Interfaces;
using Message.Domain.Contracts.Repositories;
using CSharpFunctionalExtensions;

namespace Message.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository messageRepository;
        private readonly IChatRoomRepository chatRoomRepository;

        public MessageService(
            IMessageRepository messageRepository, 
            IChatRoomRepository chatRoomRepository)
        {
            this.messageRepository = messageRepository;
            this.chatRoomRepository = chatRoomRepository;
        }

        public async Task<DataResponse<List<MessageResponse>>> GetChatMessages(string chatId)
        {
            try
            {
                var chat = await chatRoomRepository.GetChatRoom(chatId);

                if (chat.IsFailure)
                {
                    return new DataResponse<List<MessageResponse>>
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Chat room is not exist",
                        Data = new()
                    };        
                }

                var chatHistory = await messageRepository.GetChatMessages(chatId);

                return new DataResponse<List<MessageResponse>>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Success get chat history",
                    Data = chatHistory.Select(x => new MessageResponse
                    {
                        Id = x.Id,
                        ChatRoomId = x.ChatRoomId,
                        Message = x.Text,
                        SenderEmail = x.SenderEmail,
                        SendTime = x.SendTime
                    }).ToList()
                };
            }
            catch
            {
                return new DataResponse<List<MessageResponse>>()
                {
                    Data = new List<MessageResponse>(),
                    Description = "Interna Server Error",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
