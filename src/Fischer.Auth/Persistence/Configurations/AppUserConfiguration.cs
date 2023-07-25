using Fischer.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fischer.Auth.Persistence.Configurations;
public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    private const string Schema = "Identity";
    private const string User = "AppUser";

    public void Configure(EntityTypeBuilder<AppUser> builder)
    {

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired(true).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired(true);
        builder.Property(x => x.CellPhone).IsRequired(false);
        builder.Property(x => x.ShouldAlterPass).IsRequired(true);

        builder.HasMany(u => u.UserRoles)
            .WithOne(up => up.User)
            .HasForeignKey(up => up.UserId)
            .IsRequired();


        builder.ToTable(User, Schema);
    }
}