using CSharpFunctionalExtensions;
using Message.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Domain.Contracts.Repositories
{
    public interface IChatRoomRepository
    {
        Task<Result> AddChatRoom(ChatRoom chatRoom);

        Task<Result<ChatRoom>> GetChatRoom(string sellerEmail, 
                string customerEmail, 
                long productId);

        IQueryable<ChatRoom> GetUserChatRooms(string email);

        IQueryable<ChatRoom> GetSellerChatRooms(string email);

        Task<Result<ChatRoom>> GetChatRoom(string id);

        Task DeleteChatRoom(ChatRoom chatRoom);
    }
}
