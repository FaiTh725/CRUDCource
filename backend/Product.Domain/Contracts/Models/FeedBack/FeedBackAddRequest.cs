using Microsoft.AspNetCore.Http;
namespace Product.Domain.Contracts.Models.FeedBack
{
    public class FeedBackAddRequest
    {
        public long ProductId { get; set; }

        public string EmailAccount { get; set; } = string.Empty;

        public IFormFileCollection? FeedBackImages { get; set; }

        public string Text { get; set; } = string.Empty;

        public int Rate { get; set; }
    }
}
