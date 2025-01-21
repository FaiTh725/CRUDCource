using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Domain.Entities;

namespace Product.Dal.Configurations
{
    public class FeedBackConfiguration : IEntityTypeConfiguration<FeedBack>
    {
        public void Configure(EntityTypeBuilder<FeedBack> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey("ProductId")
                .IsRequired();

            builder.HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey("OwnerId")
                .IsRequired();

            builder.Property(x => x.Rate)
                .IsRequired();

            builder.ToTable(t => t
            .HasCheckConstraint(
                "CK_FeedBack_Rate", 
                $"Rate >= {FeedBack.MIN_RATE} AND Rate <= {FeedBack.MAX_RATE}"));
        }
    }
}
