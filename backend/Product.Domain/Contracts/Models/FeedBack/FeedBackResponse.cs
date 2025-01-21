using Product.Domain.Contracts.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
