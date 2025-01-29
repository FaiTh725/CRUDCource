using Product.Domain.Contracts.Models.Product;

namespace Product.Domain.Contracts.Models.Account
{
    public class AccountBuyProductRequest
    {
        public string Email { get; set; }

        public List<BuyProductRequest> Products { get; set; } = new List<BuyProductRequest>();
    }
}
