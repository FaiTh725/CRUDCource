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
    public class Message
    {
        public string Id { get; init; } = string.Empty;

        public string ChatRoomId {  get; init; } = string.Empty;

        public string Text {  get; init; } = string.Empty;

        public string SenderEmail {  get; init; } = string.Empty;

        public DateTime SendTime {  get; init; }

        public Message()
        {

        }

        private Message(
            string chatRoomId,
            string text,
            string senderEmail)
        {
            Id = Guid.NewGuid().ToString();
            SendTime = DateTime.Now;

            ChatRoomId = chatRoomId;
            Text = text;
            SenderEmail = senderEmail;
        }

        public static Result<Message> Initialize(
            string chatRoomId,
            string text,
            string senderEmail)
        {
            if(string.IsNullOrEmpty(chatRoomId))
            {
                return Result.Failure<Message>("ChatRoomId is requred value");
            }

            if(string.IsNullOrWhiteSpace(text))
            {
                return Result.Failure<Message>("Text is empty or null");
            }

            Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if(!emailRegex.IsMatch(senderEmail))
            {
                return Result.Failure<Message>("Sender email invalid signature should be email type");
            }

            return Result.Success(new Message(
                chatRoomId,
                text,
                senderEmail));
        }
    }
}
