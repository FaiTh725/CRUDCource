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

            //builder.ToTable(x => x.HasCheckConstraint("CK_ProductEntity_Count", "[Count] > 0 AND [Count] <= 1000000"));
        }
    }
}
