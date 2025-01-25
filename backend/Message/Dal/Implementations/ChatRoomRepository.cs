using CSharpFunctionalExtensions;
using Message.Domain.Contracts.Repositories;
using Message.Domain.Entities;
using Redis.OM;
using Redis.OM.Searching;
using StackExchange.Redis;

namespace Message.Dal.Implementations
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        private readonly IDatabase database;
        private readonly IRedisCollection<ChatRoom> chats;

        public ChatRoomRepository(
            IConnectionMultiplexer connectionMultiplexer)
        {
            database = connectionMultiplexer.GetDatabase();
            var redisProvider = new RedisConnectionProvider(connectionMultiplexer);
            redisProvider.Connection.CreateIndex(typeof(ChatRoom));
            chats = redisProvider.RedisCollection<ChatRoom>();
        }

        public async Task<Result> AddChatRoom(ChatRoom chatRoom)
        {

            try
            {
                await chats.InsertAsync(chatRoom);

                return Result.Success("Add new chat room");
            }
            catch
            {
                return Result.Failure("Error crate chat room");
            }
        }

        public async Task DeleteChatRoom(ChatRoom chatRoom)
        {
            await chats.DeleteAsync(chatRoom);
        }

        public async Task<Result<ChatRoom>> GetChatRoom(string sellerEmail, string customerEmail, long productId)
        {
            var chat = await chats.FirstOrDefaultAsync(x => x.SellerEmail == sellerEmail &&
                x.CustomerEmail == customerEmail &&
                x.ProductId == productId);
        
            if(chat is null)
            {
                return Result.Failure<ChatRoom>("Chat already exist");
            }

            return Result.Success(chat);
        }

        public async Task<Result<ChatRoom>> GetChatRoom(string id)
        {
            var chat = await chats
                .FirstOrDefaultAsync(x => x.Id == id);

            if(chat is null)
            {
                return Result.Failure<ChatRoom>("Chat not exist");
            }

            return Result.Success(chat);
        }

        public IQueryable<ChatRoom> GetSellerChatRooms(string email)
        {
            var sellerChats = chats
                .Where(x => x.SellerEmail == email)
                .AsQueryable();

            return sellerChats;
        }

        public IQueryable<ChatRoom> GetUserChatRooms(string email)
        {
            var userChats = chats
                .Where(x => x.CustomerEmail == email)
                .AsQueryable();

            return userChats;
        }
    }
}
