using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Domain.Entities;

namespace Product.Dal.Configurations
{
    public class ChangeRoleConfiguration : IEntityTypeConfiguration<ChangeAccountRoleRequest>
    {
        public void Configure(EntityTypeBuilder<ChangeAccountRoleRequest> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.NewRole)
                .IsRequired();
        }
    }
}
