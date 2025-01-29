
namespace Product.Domain.Contracts.Models.Product
{
    public  class BuyProductRequest
    {
        public long ProductId { get; set; }
        
        public int Count { get; set; }
    }
}
