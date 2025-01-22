using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Contracts.Models.Product
{
    public class ProductPaginationResponse
    {
        public int Page {  get; set; }

        public int Count { get; set; }

        public long MaxCount { get; set; }

        public List<ProductResponse> Products { get; set; } = new List<ProductResponse>();
    }
}
