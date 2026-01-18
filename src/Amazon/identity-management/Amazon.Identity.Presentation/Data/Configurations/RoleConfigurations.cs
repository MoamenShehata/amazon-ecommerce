using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Amazon.Identity.Presentation.Data.Configurations;

public class RoleConfigurations : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        var roles = new IdentityRole[]
        {
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            }
        };

        builder.HasData(roles);
    }
}