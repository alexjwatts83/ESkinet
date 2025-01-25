using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkitNet.Infrastructure.Data.Configurations;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasConversion(productId => productId.Value, dbId => ProductId.Of(dbId));

        builder.Property(p => p.Id).HasDefaultValueSql("NEWID()");

        builder.Property(p => p.Name).HasMaxLength(100);

        // TODO: find a better limit later on
        builder.Property(p => p.Description).HasMaxLength(1000);

        builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
    }
}
