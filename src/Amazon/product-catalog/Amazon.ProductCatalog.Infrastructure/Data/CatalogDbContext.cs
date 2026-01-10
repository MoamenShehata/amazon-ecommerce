using System.Text.Json;
using Amazon.ProductCatalog.Domain.Categories;
using Amazon.ProductCatalog.Domain.Products;
using Amazon.ProductCatalog.Domain.Products.ValueObjects;
using EMP.SharedKernel.DDD.Definitions;
using Microsoft.EntityFrameworkCore;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.ProductCatalog.Infrastructure.Data;

public class CatalogDbContext : DbContext, IUnitOfWork
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> dbContextOptions
        ) : base(dbContextOptions)
    {
    }


    public DbSet<Category> Categories { get; private set; }
    public DbSet<Product> Products { get; private set; }
    public DbSet<OutboxMessage> EventStore { get; private set; }

    public async Task CommitAsync()
    {
        var entries = ChangeTracker.Entries<IAuditableEntity>().Where(s => s.State == EntityState.Added);
        foreach (var item in entries)
        {
            item.Entity.CreatedOn = DateTime.UtcNow;
            item.Entity.CreatedBy = "Test";
        }

        await SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);

            entity.HasOne(e => e.ParentCategory)
                  .WithMany(x => x.Children)
                  .HasForeignKey(e => e.ParentCategoryId)
                  .IsRequired(false);

            entity.HasQueryFilter(p => !p.IsDeleted);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).IsRequired().HasMaxLength(300);

            entity.ComplexProperty(e => e.Price, builder =>
            {
                builder.Property(x => x.Amount);
                builder.Property(x => x.Min);
                builder.Property(x => x.Max);
            });

            entity.Property(x => x.Properties)
            .HasConversion(
            ls => JsonSerializer.Serialize(ls, (JsonSerializerOptions)null),
            json => JsonSerializer.Deserialize<List<ProductProperty>>(json, (JsonSerializerOptions)null));

            entity.HasQueryFilter(p => !p.IsDeleted);
        });

        base.OnModelCreating(modelBuilder);
    }
}