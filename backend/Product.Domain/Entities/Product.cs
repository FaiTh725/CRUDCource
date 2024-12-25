using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Models
{
    public class Product
    {
        private const int MAX_NAME_LENGTH = 30;
        public const int MAX_COUNT_PRODUCT = 1000000;

        public long Id { get; }

        public string Name { get; private set; }

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; private set; }

        public int Count { get; private set; }

        // Folder that contains all images about this product
        // Pattern like Name__Id in lowercase
        public string ImageFolder 
        {  
            get
            {
                return $"{Name}__{Id}".ToLower();
            } 
        }

        public Account Sealer { get; private set; }

        public List<Account> AccountInShopingCart { get; private set; } = new List<Account>();

        public Product()
        {

        }

        private Product(string name, Account sealer, decimal price, int count)
        {
            Name = name;
            Sealer = sealer;
            Price = price;
            Count = count;
        }

        public static Result<Product> Initialize(
                string name, 
                Account sealer, 
                decimal price, 
                int count)
        {
            if(string.IsNullOrEmpty(name) ||
               name.Length > MAX_NAME_LENGTH )
            {
                return Result.Failure<Product>("Name lenght should be less than 30 and not empty");
            }

            if(price < 0)
            {
                return Result.Failure<Product>("Product price cant be less than zero");
            }

            if(sealer == null)
            {
                return Result.Failure<Product>("The sealer of product cant be empty");
            }

            if(count < 0 || count > MAX_COUNT_PRODUCT)
            {
                return Result.Failure<Product>("Product count should be positove and not greate than " + MAX_COUNT_PRODUCT.ToString());
            }

            return Result.Success<Product>(new Product(name, sealer, price, count));
        }
    }
}
