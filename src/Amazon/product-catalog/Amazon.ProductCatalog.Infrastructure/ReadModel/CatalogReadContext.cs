using Amazon.ProductCatalog.Read.Models;
using Microsoft.EntityFrameworkCore;
using Moamen.SDKs.SharedKernel;

namespace Amazon.ProductCatalog.Infrastructure.ReadModel;

public class CatalogReadContext : DbContextBase
{
    public CatalogReadContext(DbContextOptions<CatalogReadContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductForListModel>(e =>
        {
            e.ToTable("products", "catalog.read");
        });

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<ProductForListModel> Products { get; set; }
}
