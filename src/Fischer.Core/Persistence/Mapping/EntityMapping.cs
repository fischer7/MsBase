using Fischer.Core.Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fischer.Core.Persistence.Mapping;
public abstract class EntityMapping<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : Entity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        //.HasKey(x => x.Id); // EF assumes ID as a key.
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.CreationDate).IsRequired();
        builder.Property(x => x.UpdateDate).IsRequired(false);
    }
}