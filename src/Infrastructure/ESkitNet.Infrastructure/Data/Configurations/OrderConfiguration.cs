using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkitNet.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id).HasConversion(orderId => orderId.Value, dbId => OrderId.Of(dbId));

        builder.Property(o => o.OrderDate)
            .HasConversion(
                x => x.ToUniversalTime(),
                x => DateTime.SpecifyKind(x, DateTimeKind.Utc)
            )
            .IsRequired();

        builder.Property(oi => oi.BuyerEmail).IsRequired();

        builder.OwnsOne(x => x.ShippingAddress, o => o.WithOwner());

        builder.OwnsOne(x => x.PaymentSummary, o => o.WithOwner());

        // Orders has many Items
        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(o => o.Status)
            .HasDefaultValue(OrderStatus.Pending)
            .HasConversion(
                s => s.ToString(),
                dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));

        builder.Property(oi => oi.SubTotal)
           .HasColumnType("decimal(18,2)")
           .IsRequired();

        builder.Property(oi => oi.PaymentIntentId).IsRequired();
    }
}