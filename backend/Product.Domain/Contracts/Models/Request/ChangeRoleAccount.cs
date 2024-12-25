using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Contracts.Models.Request
{
    public class ChangeRoleAccount
    {
        public string Email { get; set; }

        public string NewRole { get; set; }
    }
}
