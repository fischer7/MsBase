using Fischer.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fischer.Auth.Persistence.Configurations;
public sealed class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
{
    private const string Schema = "Identity";
    private const string UserRole = "AppUserRole";

    public void Configure(EntityTypeBuilder<AppUserRole> builder)
        => builder.ToTable(UserRole, Schema);
}
