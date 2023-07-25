using Fischer.Core.Persistence.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fischer.Audit.Persistence.Configs;
public sealed class AuditConfiguration : EntityMapping<Domain.Audit>
{
    private const string Schema = "Audit";
    private const string TableName = "Audit";
    public override void Configure(EntityTypeBuilder<Domain.Audit> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.AuditKind)
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(x => x.EntityName)
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(x => x.TableName)
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.HasMany(x => x.Props)
            .WithOne(p => p.Audit)
            .HasForeignKey(p => p.AuditId);

        builder.ToTable(TableName, Schema);
    }
}
