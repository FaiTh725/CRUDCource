using Product.Domain.Contracts.Models.Product;
namespace Product.Domain.Contracts.Models.Account
{
    public class AccountResponseCartItems : AccountResponseDetail
    {
        public List<ProductCartResponse> CartProducts { get; set; } = new List<ProductCartResponse>();
    }
}
