namespace Product.Dal.Interfaces
{
    public interface IDatabaseTransaction
    {
        Task StartTransaction();

        Task CommitTransaction();

        Task RollBackTransaction();

        Task Dispose();
    }
}
