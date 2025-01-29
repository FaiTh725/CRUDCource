using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductEntity = Product.Domain.Models.Product;

namespace Product.Dal.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasOne(x => x.Sealer)
                .WithMany(x => x.SoldProducts);

            builder.Property(x => x.Count)
                .IsRequired();
        }
    }
}
