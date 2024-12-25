using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Contracts.Models.Account
{
    public class CreateAccount
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }
}
