using Product.Domain.Contracts.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Contracts.Models.Account
{
    public class AccountResponseOrderHisory : AccountResponseDetail
    {
        public List<ProductResponse> OrderHistory { get; set; } = new List<ProductResponse>();
    }
}
