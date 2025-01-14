//using CSharpFunctionalExtensions;
//using Message.Domain.Contracts.Repositories;
//using Message.Domain.Entities;
//using StackExchange.Redis;
//using System.Diagnostics.CodeAnalysis;
//using System.Text.Json;

//namespace Message.Dal.Implementations
//{
//    public class ChatUserRepository : IChatUserRepository
//    {
//        private readonly IDatabase db;
//        private readonly IServer server;

//        public ChatUserRepository(IConnectionMultiplexer redis)
//        {
//            db = redis.GetDatabase();

//            var endpoint = redis.GetEndPoints().First();
//            server = redis.GetServer(endpoint);
//        }

//        public async Task<Result> AddChatUser(ChatUser chatUser)
//        {
//            try
//            {
//                await db.StringSetAsync(
//                    $"Users:{chatUser.Email}",
//                    JsonSerializer.Serialize(chatUser));

//                return Result.Success("Create New Chat User");
//            }
//            catch
//            {
//                return Result.Failure("Error add new user");
//            }
//        }

//        public async Task<Result> AddConnectionId(ChatUser chatUser, string connectionId)
//        {
//            try
//            {
//                await db.ListRightPushAsync(
//                    $"Users:" +
//                    $"{chatUser.Email}:" +
//                    $"listConnectionId", 
//                    connectionId);

//                return Result.Success("Add new connection Id");
//            }
//            catch
//            {
//                return Result.Failure("Error Add new connection id");
//            }
//        }

//        public async Task<Result<ChatUser>> GetChatUser(string email)
//        {
//            try
//            {
//                var jsonResult = await db.StringGetAsync($"Users:{email}");

//                return jsonResult.HasValue ?
//                    Result.Success(JsonSerializer.Deserialize<ChatUser>(jsonResult!)) :
//                    Result.Failure<ChatUser>("User not exist");
//            }
//            catch
//            {
//                return Result.Failure<ChatUser>("Error find new chat user");
//            }
//        }

//        public async Task<Result<List<string>>> GetUserListConnectionId(ChatUser chatUser)
//        {
//            try
//            {
//                var jsonData = await db.ListRangeAsync(
//                    "Users:" +
//                    $"{chatUser.Email}:" +
//                    $"listConnectionId");
                
//                return jsonData.Select(x => x.ToString()).ToList();
//            }
//            catch
//            {
//                return Result.Failure<List<string>>("Error get connection id users");
//            }
//        }

//        public async Task<Result> RemoveConnectionId(ChatUser chatUserm, string connectionId)
//        {
//            try
//            {
//                await db.ListRemoveAsync(
//                    $"Users:" +
//                    $"{chatUserm.Email}:" +
//                    $"listConnectionId",
//                    connectionId);

//                return Result.Success("Delete connection Id");
//            }
//            catch
//            {
//                return Result.Failure("Error execute connection id");
//            }
//        }
//    }
//}
