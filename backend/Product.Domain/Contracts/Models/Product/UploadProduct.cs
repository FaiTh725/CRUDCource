using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Contracts.Models.Product
{
    public class UploadProduct
    {
        public IFormFileCollection? Files { get; set; }

        public string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string SealerEmail { get; set; }

        public int Count { get; set; }
    }
}
