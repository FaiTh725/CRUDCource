using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Product.Domain.Contracts.Repositories;
using System.Reflection.Metadata.Ecma335;
using ProductEntity = Product.Domain.Models.Product;

namespace Product.Dal.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext context;

        public ProductRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<ProductEntity>> AddProduct(ProductEntity product)
        {
            var newProductEntity = await context.Products.AddAsync(product);
        
            var countChanges = await context.SaveChangesAsync();

            return countChanges > 0 ?
                Result.Success<ProductEntity>(newProductEntity.Entity) :
                Result.Failure<ProductEntity>("Error add Product");
        }

        public async Task<Result<ProductEntity>> GetProduct(long id)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

            return product is not null ?
                Result.Success<ProductEntity>(product) :
                Result.Failure<ProductEntity>("Product not exist");
        }

        public IQueryable<ProductEntity> GetProducts()
        {
            return context.Products.AsQueryable();
        }

        public async Task<Result<ProductEntity>> GetProductWithSeller(long productId)
        {
            var product = await context.Products
                .Include(x => x.Sealer)
                .FirstOrDefaultAsync(x => x.Id == productId);

            return product is null ?
                Result.Failure<ProductEntity>("Product dont found") :
                Result.Success(product);
        }

        public async Task<Result> UpdateProducts(List<ProductEntity> products)
        {
            context.Products.UpdateRange(products);

            var countChanges = await context.SaveChangesAsync();

            return countChanges > 0 ?
                Result.Success() :
                Result.Failure("Error update products count");
        }
    }
}
