
namespace Product.Domain.Contracts.Models.Request
{
    public class CommitRequestChangeRole
    {
        public bool StatusRequest { get; set; }

        public long RequestId { get; set; }
    }
}
