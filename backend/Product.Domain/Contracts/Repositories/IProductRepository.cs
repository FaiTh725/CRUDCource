using CSharpFunctionalExtensions;
using ProductEntity = Product.Domain.Models.Product;

namespace Product.Domain.Contracts.Repositories
{
    public interface IProductRepository
    {
        Task<Result<ProductEntity>> AddProduct(ProductEntity product);

        Task<Result<ProductEntity>> GetProduct(long id);

        IQueryable<ProductEntity> GetProducts();

        Task<Result> UpdateProducts(List<ProductEntity> products);

        Task<Result<ProductEntity>> GetProductWithSeller(long productId);
    }
}
