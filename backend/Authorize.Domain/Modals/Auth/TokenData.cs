﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Domain.Modals.Auth
{
    public class TokenData
    {
        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty ;

        public string Name { get; set; } = string.Empty;
    }
}
