using CSharpFunctionalExtensions;
using Product.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductEntity = Product.Domain.Models.Product;

namespace Product.Domain.Contracts.Repositories
{
    public interface IProductRepository
    {
        Task<Result<ProductEntity>> AddProduct(ProductEntity product);

        Task<Result<ProductEntity>> GetProduct(long id);

        Task<Result<ProductEntity>> GetProductWithShopingCart(long id);

        Result<IQueryable<ProductEntity>> GetProducts();
    }
}
