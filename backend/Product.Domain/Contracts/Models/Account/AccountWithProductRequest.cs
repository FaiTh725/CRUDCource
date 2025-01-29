
namespace Product.Domain.Contracts.Models.Account
{
    public class AccountWithProductRequest
    {
        public long ProductId { get; set; }

        public string Email { get; set; }
    }
}
