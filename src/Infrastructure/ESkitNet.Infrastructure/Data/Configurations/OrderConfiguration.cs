using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkitNet.Infrastructure.Data.Configurations;

public class IEntityConfiguration<T> : IEntityTypeConfiguration<Entity<T>>
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

        //builder.ComplexProperty(
        //    o => o.ShippingAddress, nameBuilder =>
        //    {
        //        nameBuilder.Property(n => n.Name)
        //            .IsRequired();
        //        nameBuilder.Property(n => n.Line1)
        //            .IsRequired();
        //        nameBuilder.Property(n => n.Line2);
        //        nameBuilder.Property(n => n.City).IsRequired();
        //        nameBuilder.Property(n => n.State).IsRequired();
        //        nameBuilder.Property(n => n.PostalCode).IsRequired();
        //        nameBuilder.Property(n => n.Country).IsRequired();

        //    });

        builder.ComplexProperty(
            o => o.DeliveryMethod, nameBuilder =>
            {
                nameBuilder.Property(n => n.DeliveryMethodId).IsRequired();
                nameBuilder.Property(n => n.ShortName).IsRequired();
                nameBuilder.Property(n => n.DeliveryTime).IsRequired();
                nameBuilder.Property(n => n.Description).IsRequired();
                nameBuilder.Property(n => n.Price).HasColumnType("decimal(18,2)").IsRequired();
            });

        builder.OwnsOne(x => x.PaymentSummary, o => o.WithOwner());
        //builder.ComplexProperty(
        //    o => o.PaymentSummary, nameBuilder =>
        //    {
        //        nameBuilder.Property(n => n.Last4).IsRequired();
        //        nameBuilder.Property(n => n.Brand).IsRequired();
        //        nameBuilder.Property(n => n.ExpMonth).IsRequired();
        //        nameBuilder.Property(n => n.ExpYear).IsRequired();
        //    });

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