using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkitNet.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id).HasConversion(orderId => orderId.Value, dbId => OrderId.Of(dbId));

        builder.Property(oi => oi.OrderDate).IsRequired();

        builder.Property(oi => oi.BuyerEmail).IsRequired();

        builder.ComplexProperty(
            o => o.ShippingAddress, nameBuilder =>
            {
                nameBuilder.Property(n => n.Name)
                    .IsRequired();
                nameBuilder.Property(n => n.Line1)
                    .IsRequired();
                nameBuilder.Property(n => n.Line2);
                nameBuilder.Property(n => n.City).IsRequired();
                nameBuilder.Property(n => n.State).IsRequired();
                nameBuilder.Property(n => n.PostalCode).IsRequired();
                nameBuilder.Property(n => n.Country).IsRequired();

            });

        builder.ComplexProperty(
            o => o.DeliveryMethod, nameBuilder =>
            {
                nameBuilder.Property(n => n.DeliveryMethodId).IsRequired();
                nameBuilder.Property(n => n.ShortName).IsRequired();
                nameBuilder.Property(n => n.DeliveryTime).IsRequired();
                nameBuilder.Property(n => n.Description).IsRequired();
                nameBuilder.Property(n => n.Price).HasColumnType("decimal(18,2)").IsRequired();
            });

        builder.ComplexProperty(
            o => o.PaymentSummary, nameBuilder =>
            {
                nameBuilder.Property(n => n.Last4).IsRequired();
                nameBuilder.Property(n => n.Brand).IsRequired();
                nameBuilder.Property(n => n.ExpMonth).IsRequired();
                nameBuilder.Property(n => n.ExpYear).IsRequired();
            });

        // Orders has many Items
        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId);

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