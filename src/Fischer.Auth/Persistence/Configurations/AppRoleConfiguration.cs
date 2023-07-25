using Fischer.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fischer.Auth.Persistence.Configurations;
public sealed class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    private const string Schema = "Identity";
    private const string Role = "AppRole";

    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.ToTable(Role, Schema);

        builder.HasMany(p => p.RoleClaims)
            .WithOne(pp => pp.Role)
            .HasForeignKey(pp => pp.RoleId)
            .IsRequired();
        builder.HasMany(p => p.UserRoles)
            .WithOne(ro => ro.Role)
            .HasForeignKey(r => r.RoleId)
            .IsRequired();
    }
}
