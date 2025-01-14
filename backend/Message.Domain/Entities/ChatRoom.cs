using CSharpFunctionalExtensions;
using Redis.OM.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Message.Domain.Entities
{

    [Document(StorageType = StorageType.Json,
        IndexName = "ChatRoom", 
        Prefixes = new[] {"Chats"})]
    public class ChatRoom
    {
        [RedisField]
        [RedisIdField]
        [Indexed]
        public string Id { get; init; } = string.Empty;

        [Indexed]
        public string SellerEmail {  get; init; } = string.Empty;

        [Indexed]
        public string CustomerEmail {  get; init; } = string.Empty;

        [Indexed]
        public long ProductId { get; init; }

        [Indexed]
        public string NameProduct {  get; init; } = string.Empty;

        public ChatRoom() { }

        private ChatRoom(
            string sellerEmail,
            string customerEmail,
            long productId,
            string nameProduct)
        {
            Id = Guid.NewGuid().ToString();

            SellerEmail = sellerEmail;
            CustomerEmail = customerEmail;
            ProductId = productId;
            NameProduct = nameProduct;
        }

        public static Result<ChatRoom> Initialize(
            string sellerEmail,
            string customerEmail,
            long productId,
            string nameProduct)
        {
            Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if(!emailRegex.IsMatch(sellerEmail) ||
                !emailRegex.IsMatch(customerEmail))
            {
                return Result.Failure<ChatRoom>("Invalid seller or customer email");
            }

            if(string.IsNullOrEmpty(nameProduct))
            {
                return Result.Failure<ChatRoom>("Name product shouldnt empty");
            }

            if(productId <= 0)
            {
                return Result.Failure<ChatRoom>("ProductId is invalid");
            }

            return Result.Success(new ChatRoom(
                sellerEmail, 
                customerEmail, 
                productId,
                nameProduct));
        }
    }
}
