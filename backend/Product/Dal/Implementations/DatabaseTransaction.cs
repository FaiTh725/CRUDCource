using Microsoft.EntityFrameworkCore.Storage;
using Product.Dal.Interfaces;

namespace Product.Dal.Implementations
{
    public class DatabaseTransaction : IDatabaseTransaction
    {
        private readonly AppDbContext context;
        private IDbContextTransaction transaction;

        public DatabaseTransaction(AppDbContext context)
        {
            this.context = context;
        }
        public async Task CommitTransaction()
        {
            await transaction.CommitAsync();
        }

        public async Task Dispose()
        {
            await context.DisposeAsync();
        }

        public async Task RollBackTransaction()
        {
            await transaction.RollbackAsync();  
        }

        public async Task StartTransaction()
        {
            transaction = await context.Database.BeginTransactionAsync();
        }
    }
}
