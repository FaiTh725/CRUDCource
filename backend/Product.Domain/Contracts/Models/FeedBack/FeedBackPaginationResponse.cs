
namespace Product.Domain.Contracts.Models.FeedBack
{
    public class FeedBackPaginationResponse
    {
        public List<FeedBackResponse> FeedBacks { get; set; } = new List<FeedBackResponse>();

        public int Page {  get; set; }

        public int Count { get; set; }

        public int MaxCount { get; set; }
    }
}
