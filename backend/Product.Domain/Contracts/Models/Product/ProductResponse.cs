using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Contracts.Models.Product
{
    public class ProductResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Count { get; set; }

        public List<string> Images { get; set; } = new List<string>();
    }
}
