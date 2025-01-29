using Microsoft.AspNetCore.Http;

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
