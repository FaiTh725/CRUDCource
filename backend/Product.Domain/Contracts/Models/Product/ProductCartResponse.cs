﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Contracts.Models.Product
{
    public class ProductCartResponse : ProductResponse
    {
        public int MaxCountProduct { get; set; }
    }
}
