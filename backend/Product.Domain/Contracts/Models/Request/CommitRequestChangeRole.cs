using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Contracts.Models.Request
{
    public class CommitRequestChangeRole
    {
        public bool StatusRequest { get; set; }

        public long RequestId { get; set; }
    }
}
