using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkitNet.Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        var adminRole = BuildIdentityRole("Admin");
        var customerRole = BuildIdentityRole("Customer");

        builder.HasData(adminRole, customerRole);
    }

    private static IdentityRole BuildIdentityRole(string roleName)
    {
        return new IdentityRole()
        {
            Id = Guid.NewGuid().ToString(),
            Name = roleName,
            NormalizedName = roleName.ToUpper()
        };
    }
}