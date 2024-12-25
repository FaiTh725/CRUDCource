using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Domain.Models;

namespace Product.Dal.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.Email)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasMany(x => x.SoldProducts)
                .WithOne(x => x.Sealer)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.ShopingCart)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
