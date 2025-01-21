using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Contracts.Models.Product
{
    public class ProductRating
    {
        public long ProductId { get; set; }

        public double Rating { get; set; }
    }
}
