using CSharpFunctionalExtensions;
using Message.Domain.Contracts.Modals.ChatRoom;
using Message.Domain.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc.Formatters;
using Redis.OM.Searching.Query;
using StackExchange.Redis;
using System.Text.Json;
using MessageEntity = Message.Domain.Entities.Message;

namespace Message.Dal.Implementations
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IDatabase database;
        private readonly IServer server;

        public MessageRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            database = connectionMultiplexer.GetDatabase();
            var endpoint = connectionMultiplexer.GetEndPoints().First();
            var server = connectionMultiplexer.GetServer(endpoint);
        }

        private string GetMessageKey(string chatId) =>
            $"Messages:{chatId}";

        public async Task<Result> AddMessage(MessageEntity messageEntity)
        {
            try
            {
                await database.ListLeftPushAsync(
                    GetMessageKey(messageEntity.ChatRoomId),
                    JsonSerializer.Serialize(messageEntity));

                return Result.Success("Add new Message");
            }
            catch
            {
                return Result.Failure("Error add message");
            }
        }

        public async Task<List<MessageEntity>> GetChatMessages(string chatId)
        {
            var messagesJson = await database.ListRangeAsync(
                GetMessageKey(chatId));

            var messages = new List<MessageEntity>();

            foreach (var message in messagesJson)
            {
                messages.Add(JsonSerializer.Deserialize<MessageEntity>(message!)
                    ?? throw new NullReferenceException("Error deserialize"));
            }

            return messages;
            
        }

        public async Task<bool> DeleteChatHistory(string chatId)
        {
            return await database.KeyDeleteAsync(GetMessageKey(chatId));
        }
    }
}
