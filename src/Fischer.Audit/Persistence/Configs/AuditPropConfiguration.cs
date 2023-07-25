using Fischer.Audit.Domain;
using Fischer.Core.Persistence.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fischer.Audit.Persistence.Configs;
public sealed class AuditPropConfiguration : EntityMapping<AuditProp>
{
    private const string Schema = "Audit";
    private const string TableName = "AuditProps";
    public override void Configure(EntityTypeBuilder<AuditProp> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.PropertyName)
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(x => x.OldValue)
            .HasColumnType("text");

        builder.Property(x => x.NewValue)
            .HasColumnType("text");
        builder.ToTable(TableName, Schema);
    }
}