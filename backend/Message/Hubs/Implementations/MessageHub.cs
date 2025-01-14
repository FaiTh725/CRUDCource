using Application.Contracts.Response;
using CSharpFunctionalExtensions;
using Message.Domain.Contracts.Modals.ChatRoom;
using Message.Domain.Contracts.Modals.Message;
using Message.Domain.Contracts.Repositories;
using Message.Domain.Entities;
using Message.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Product.Domain.Contracts.Models.Account;
using System.Text.Json;
using MessageEntiy = Message.Domain.Entities.Message;

namespace Message.Hubs.Implementations
{
    public class MessageHub : Hub<IMessageHub>
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;
        private readonly ILogger<MessageHub> logger;
        private readonly IChatRoomRepository chatRoomRepository;
        private readonly IMessageRepository messageRepository;

        public MessageHub(
            HttpClient httpClient,
            IConfiguration configuration, 
            ILogger<MessageHub> logger,
            IChatRoomRepository chatRoomRepository,
            IMessageRepository messageRepository)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
            this.logger = logger;
            this.chatRoomRepository = chatRoomRepository;
            this.messageRepository = messageRepository;
        }

        // Issues
        // May should add check when user(seller) conected is exist in product ip

        public async Task CreateNewChat(NewChat connection)
        {
            try
            {
                var baseUrlProductApi = configuration.GetValue<string>("ApiList:ProductApi")
                    ?? throw new NullReferenceException();

                var request = await httpClient.GetAsync($"{baseUrlProductApi}" +
                    $"/Product/ProductSeller?id={connection.ProductId}");

                if(!request.IsSuccessStatusCode)
                {
                    await Clients.Caller.ChatCreatedResult(new());
                    return;
                }

                var responseJson = await request.Content.ReadAsStringAsync();

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var responseData = JsonSerializer.Deserialize<DataResponse<ProductSeller>>(
                    responseJson, jsonOptions)
                    ?? throw new JsonException();

                if (responseData.StatusCode != StatusCode.Ok)
                {
                    await Clients.Caller.ChatCreatedResult(new());
                    return;
                }

                if(responseData.Data.Email ==
                    connection.ConsumerEmail)
                {
                    await Clients.Caller.ChatCreatedResult(new());
                    logger.LogError("Create chat with yourself");
                    return;
                }

                var existChat = await chatRoomRepository.GetChatRoom(
                    responseData.Data.Email,
                    connection.ConsumerEmail,
                    connection.ProductId);

                if(existChat.IsSuccess)
                {
                    await Clients.Caller.ChatCreatedResult(new ());
                    
                    return;
                }

                var newChatRoom = ChatRoom.Initialize(
                    responseData.Data.Email,
                    connection.ConsumerEmail,
                    connection.ProductId,
                    connection.ProductName);

                if(newChatRoom.IsFailure)
                {
                    await Clients.Caller.ChatCreatedResult(new());
                    logger.LogError(newChatRoom.Error);
                    return;
                }

                var resultAddNewChat = await chatRoomRepository
                    .AddChatRoom(newChatRoom.Value);

                if(resultAddNewChat.IsFailure)
                {
                    await Clients.Caller.ChatCreatedResult(new());
                    logger.LogError("Error create new chat");
                    return;
                }

                var chatCreatedResponse = new ChatRoomResponse
                {
                    Id = newChatRoom.Value.Id,
                    CustomerEmail = newChatRoom.Value.CustomerEmail,
                    SellerEmail = newChatRoom.Value.SellerEmail,
                    ProductId = newChatRoom.Value.ProductId,
                    ProductName = newChatRoom.Value.NameProduct
                };

                await Clients.User(connection.ConsumerEmail)
                    .ChatCreatedResult(chatCreatedResponse);

                await Clients.User(responseData.Data.Email)
                    .ChatCreatedResult(chatCreatedResponse);
            }
            catch
            {
                logger.LogError("Error create chat");
            }
        }

        public async Task CloseDispute(string chatId)
        {
            try
            {
                var chat = await chatRoomRepository.GetChatRoom(chatId);

                if(chat.IsFailure)
                {
                    logger.LogError("Invalid chat id");
                    return;
                }

                await chatRoomRepository.DeleteChatRoom(chat.Value);

                await messageRepository.DeleteChatHistory(chat.Value.Id);

                await Clients
                    .User(chat.Value.CustomerEmail)
                    .ChatDeleted(chatId);

                await Clients
                    .User(chat.Value.SellerEmail)
                    .ChatDeleted(chatId);
            }
            catch
            {
                logger.LogError("Error close dipute");
            }
        }

        public async Task ConnectSeller()
        {
            try
            {
                var seller = Context.UserIdentifier;

                if (seller is null)
                {
                    await Clients.Caller.SellerConnected(new List<ChatRoomResponse>());
                    return;
                }

                var chats = chatRoomRepository
                    .GetSellerChatRooms(seller)
                    .ToList();

                var chatWithHistory = await Task.WhenAll(
                    chats.Select(async x => new ChatRoomResponse
                    {
                        Id = x.Id,
                        CustomerEmail = x.CustomerEmail,
                        ProductId = x.ProductId,
                        ProductName = x.NameProduct,
                        SellerEmail = x.SellerEmail,
                        Messages = (await messageRepository
                        .GetChatMessages(x.Id))
                        .Select(y => new MessageResponse
                        {
                            Id = y.Id,
                            ChatRoomId = y.ChatRoomId,
                            Message = y.Text,
                            SenderEmail = y.SenderEmail,
                            SendTime = y.SendTime
                        }).ToList()
                    }).ToList());

                await Clients.Caller.SellerConnected(chatWithHistory.ToList());
            }
            catch
            {
                logger.LogError("Error execute seller connected");
            }
        }

        public async Task UserConnected()
        {
            try
            {

                var client = Context.UserIdentifier;

                if(client is null)
                {
                    await Clients.Caller.UserConnected(new List<ChatRoomResponse>());
                    return;
                }

                var chats = chatRoomRepository
                    .GetUserChatRooms(client)
                    .ToList();

                var chatsWitContent = await Task
                    .WhenAll(chats.Select(async x =>
                        new ChatRoomResponse
                        {
                            Id = x.Id,
                            CustomerEmail = x.CustomerEmail,
                            SellerEmail = x.SellerEmail,
                            ProductId = x.ProductId,
                            ProductName = x.NameProduct,
                            Messages =  (await messageRepository
                                .GetChatMessages(x.Id)).Select(y => new MessageResponse
                                {
                                    Id = y.Id,
                                    ChatRoomId = y.ChatRoomId,
                                    Message = y.Text,
                                    SenderEmail = y.SenderEmail,
                                    SendTime = y.SendTime,
                                }).ToList()
                        }
                ));

                await Clients.Caller.UserConnected(chatsWitContent.ToList());
            }
            catch
            {
                logger.LogError("error processing user connected");
            }
        }

        public async Task SendMessages(SendMessage sendMessage)
        {
            try
            {
                var chat = await chatRoomRepository.GetChatRoom(sendMessage.ChatId);

                if(chat.IsFailure)
                {
                    logger.LogError("Invalid chatid, chat isnt exist");
                    return;
                }

                var newMessage = MessageEntiy.Initialize(
                    sendMessage.ChatId, 
                    sendMessage.Text, 
                    sendMessage.SenderEmail);

                if(newMessage.IsFailure)
                {
                    logger.LogError("Error create instance of message - " +
                        newMessage.Error);
                    return;
                }

                var createNewMessageResult = await messageRepository
                    .AddMessage(newMessage.Value);

                if(createNewMessageResult.IsFailure)
                {
                    logger.LogError("Error save message - " +
                        createNewMessageResult.Error);
                    return;
                }

                var messageResponse = new MessageResponse
                {
                    Id = newMessage.Value.Id,
                    Message = newMessage.Value.Text,
                    SendTime = newMessage.Value.SendTime,
                    ChatRoomId = newMessage.Value.ChatRoomId,
                    SenderEmail = newMessage.Value.SenderEmail
                };

                await Clients.User(chat.Value.CustomerEmail)
                    .MessageSent(messageResponse);

                await Clients.User(chat.Value.SellerEmail)
                    .MessageSent(messageResponse);
            }
            catch
            {
                logger.LogError("Error send message");
            }
        }
    }
}
