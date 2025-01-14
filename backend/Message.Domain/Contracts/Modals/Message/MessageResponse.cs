using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Domain.Contracts.Modals.Message
{
    public class MessageResponse
    {
        public string Id { get; set; } = string.Empty;

        public string ChatRoomId { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime SendTime { get; set; }

        public string SenderEmail { get; set; } = string.Empty;
    }
}
