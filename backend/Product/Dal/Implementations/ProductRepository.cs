using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Product.Domain.Contracts.Repositories;
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

        public Result<IQueryable<ProductEntity>> GetProducts()
        {
            var products = context.Products.AsQueryable();

            return Result.Success(products);
        }

        public async Task<Result<ProductEntity>> GetProductWithShopingCart(long id)
        {
            var product = await context.Products
                .Include(p => p.AccountInShopingCart)
                .FirstOrDefaultAsync(p => p.Id == id);
        
            return product is null ?
                Result.Failure<ProductEntity>("Product not exist") :
                Result.Success<ProductEntity>(product);
        }
    }
}
