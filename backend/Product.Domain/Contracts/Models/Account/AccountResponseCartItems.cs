using Product.Domain.Contracts.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Contracts.Models.Account
{
    public class AccountResponseCartItems : AccountResponseDetail
    {
        public List<ProductResponse> CartProducts { get; set; } = new List<ProductResponse>();
    }
}
