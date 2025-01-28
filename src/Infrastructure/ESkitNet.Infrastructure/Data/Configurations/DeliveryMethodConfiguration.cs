using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkitNet.Infrastructure.Data.Configurations;

public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasConversion(productId => productId.Value, dbId => DeliveryMethodId.Of(dbId));

        builder.Property(p => p.Id).HasDefaultValueSql("NEWID()");

        builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
    }
}
