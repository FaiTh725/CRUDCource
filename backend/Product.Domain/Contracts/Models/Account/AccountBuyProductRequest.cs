using Product.Domain.Contracts.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Contracts.Models.Account
{
    public class AccountBuyProductRequest
    {
        public string Email { get; set; }

        public List<BuyProductRequest> Products { get; set; } = new List<BuyProductRequest>();
    }
}
