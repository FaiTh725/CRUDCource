using MassTransit;
using Microsoft.EntityFrameworkCore;
using Product.Dal.Configurations;
using Product.Domain.Entities;
using Product.Domain.Models;
using Product.Features.Exceptions;
using ProductEntity = Product.Domain.Models.Product;

namespace Product.Dal
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IConfiguration configuration) :
            base(options)
        {
            this.configuration = configuration;    
        }


        public DbSet<ProductEntity> Products { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<ChangeAccountRoleRequest> ChangeRoleRequests { get; set; }

        public DbSet<FeedBack> FeedBacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ChangeRoleConfiguration());
            modelBuilder.ApplyConfiguration(new CartItemConfiguration());
            modelBuilder.ApplyConfiguration(new FeedBackConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = configuration.GetConnectionString("SQLServerConnection");

            if(connectionString is null)
            {
                throw new AplicationConfigurationException(
                    "Error Aplication Configuration",
                    "DbConnection");
            }

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
