using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkitNet.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Id).HasConversion(orderItemId => orderItemId.Value, dbId => OrderItemId.Of(dbId));

        builder.ComplexProperty(
            o => o.ItemOrdered, nameBuilder =>
            {
                nameBuilder.Property(n => n.ProductId)
                    .IsRequired();

                nameBuilder.Property(n => n.ProductName)
                    .IsRequired();

                nameBuilder.Property(n => n.PictureUrl)
                    .IsRequired();

                nameBuilder.Property(n => n.Type)
                    .IsRequired();
                nameBuilder.Property(n => n.Brand)
                    .IsRequired();
            });

        builder.Property(oi => oi.Quantity).IsRequired();

        builder.Property(oi => oi.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
    }
}
