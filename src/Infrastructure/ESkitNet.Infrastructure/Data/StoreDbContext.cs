using Microsoft.EntityFrameworkCore;

namespace ESkitNet.Infrastructure.Data;

public class StoreDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {

    }
}