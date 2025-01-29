using Product.Domain.Contracts.Models.Account;

namespace Product.Domain.Contracts.Models.FeedBack
{
    public class FeedBackResponse
    {
        public long Id { get; set; }

        public string FeedBackText { get; set; } = string.Empty;

        public List<string> Images { get; set; } = new List<string>();

        public ProductSeller OwnerFeedBack { get; set; }

        public long ProductId { get; set; }

        public int Rate { get; set; }

        public DateTime SendTime { get; set; }
    }
}
