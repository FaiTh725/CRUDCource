
namespace Product.Domain.Contracts.Models.Account
{
    public class AccountWithProductCountRequest : AccountWithProductRequest
    {
        public int Count { get; set; }
    }
}
