using Fischer.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fischer.Auth.Persistence.Configurations;
public sealed class AppRoleClaimConfiguration : IEntityTypeConfiguration<AppRoleClaim>
{
    private const string Schema = "Identity";
    private const string RoleClaim = "AppRoleClaim";

    public void Configure(EntityTypeBuilder<AppRoleClaim> builder)
        => builder.ToTable(RoleClaim, Schema);
}