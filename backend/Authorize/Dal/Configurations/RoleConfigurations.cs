using Authorize.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorize.Dal.Configurations
{
    public class RoleConfigurations : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.HasKey(roles => roles.Role);

            builder.HasMany(x => x.Users)
                .WithOne(x => x.Role);
        }
    }
}
