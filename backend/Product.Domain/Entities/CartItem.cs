using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductEntity = Product.Domain.Models.Product;

namespace Product.Domain.Entities
{
    public class CartItem
    {
        public long Id { get; }

        public int Count { get; set; }

        public DateTime DateAdd { get; }

        public ProductEntity Product { get; init; }

        public CartItem() { }

        private CartItem(int count, ProductEntity product) 
        { 
            DateAdd = DateTime.Now;
            Count = count;
            Product = product;
        }

        public static Result<CartItem> Initialize(int count, ProductEntity product)
        {
            if(count < 0 || count > ProductEntity.MAX_COUNT_PRODUCT)
            {
                return Result.Failure<CartItem>("Invalid count value, sohuld ne in range from 0 to " + 
                    ProductEntity.MAX_COUNT_PRODUCT.ToString());
            }

            if(product is null)
            {
                return Result.Failure<CartItem>("Product should not be null");
            }

            return Result.Success(new CartItem(count, product));  
        }

        public override bool Equals(object? obj)
        {
            if (obj is CartItem other)
            {
                return other.Id == Id;
            }

            return false;
        }
    }
}
