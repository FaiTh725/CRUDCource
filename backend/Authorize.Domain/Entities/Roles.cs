using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Domain.Entities
{
    public class Roles
    {
        public string Role { get; set; }

        public List<User> Users { get; set; }

    }
}
