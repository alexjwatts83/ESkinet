using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkitNet.Infrastructure.Data.Configurations;

public class EntityConfiguration<T> : IEntityTypeConfiguration<Entity<T>>
{
    public void Configure(EntityTypeBuilder<Entity<T>> builder)
    {
        builder.Property(o => o.CreatedAt)
            .HasConversion(
                x => toUniversalTime(x),
                x => toSpecificKind(x)
            )
            .IsRequired();
    }

    private DateTime? toSpecificKind(DateTime? x)
    {
        if (x.HasValue)
            return DateTime.SpecifyKind(x.Value, DateTimeKind.Utc);

        return null;
    }

    private DateTime? toUniversalTime(DateTime? x)
    {
        if (x.HasValue)
            return x.Value.ToUniversalTime();

        return null;
    }
}
