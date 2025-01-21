using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Contracts.Models.Account
{
    public class AccountTransactions
    {
        public string Email { get; set; } = string.Empty;

        public double Transactions { get; set; }
    }
}
