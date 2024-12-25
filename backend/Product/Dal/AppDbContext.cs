using Microsoft.EntityFrameworkCore;
using Product.Dal.Configurations;
using Product.Domain.Entities;
using Product.Domain.Models;
using ProductEntity = Product.Domain.Models.Product;

namespace Product.Dal
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public AppDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;    
        }


        public DbSet<ProductEntity> Products { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<ChangeAccountRoleRequest> ChangeRoleRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ChangeRoleConfiguration());
            modelBuilder.ApplyConfiguration(new CartItemConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("SQLServerConnection"));
        }
    }
}
