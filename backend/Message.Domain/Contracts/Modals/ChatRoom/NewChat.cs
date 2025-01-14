using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Domain.Contracts.Modals.ChatRoom
{
    public class NewChat
    {
        public string ConsumerEmail { get; set; } = string.Empty;

        public long ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;
    }
}
