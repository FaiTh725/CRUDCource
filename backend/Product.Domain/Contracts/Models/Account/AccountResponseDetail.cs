using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Contracts.Models.Account
{
    public class AccountResponseDetail
    {
        public string Email { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}
