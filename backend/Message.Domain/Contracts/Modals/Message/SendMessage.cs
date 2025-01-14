using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Domain.Contracts.Modals.Message
{
    public class SendMessage
    {
        public string Text { get; set; } = string.Empty;

        public string SenderEmail { get; set; } = string.Empty;

        public string ChatId {  get; set; } = string.Empty;
    }
}
