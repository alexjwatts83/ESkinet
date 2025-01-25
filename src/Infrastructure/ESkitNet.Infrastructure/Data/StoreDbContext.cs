using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ESkitNet.Infrastructure.Data;

public class StoreDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}