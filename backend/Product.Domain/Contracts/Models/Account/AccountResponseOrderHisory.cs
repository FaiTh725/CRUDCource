using Product.Domain.Contracts.Models.Product;
namespace Product.Domain.Contracts.Models.Account
{
    public class AccountResponseOrderHisory : AccountResponseDetail
    {
        public List<ProductResponse> OrderHistory { get; set; } = new List<ProductResponse>();
    }
}
