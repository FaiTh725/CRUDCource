using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Contracts.Models.Account
{
    public class AccountWithProductRequest
    {
        public long ProductId { get; set; }

        public string Email { get; set; }
    }
}
