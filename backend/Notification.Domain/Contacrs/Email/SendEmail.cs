using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Contacrs.Email
{
    public class SendEmail
    {
        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public List<string> Recipients { get; set; } = new List<string>();
    }
}
