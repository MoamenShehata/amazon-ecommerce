using Media.Domain;
using Microsoft.EntityFrameworkCore;
using Moamen.SDKs.SharedKernel;

namespace Media.Infrastructure.Data;

public class MediaContext : DbContextBase
{
    public MediaContext(DbContextOptions<MediaContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Media>(e =>
        {
            e.ComplexProperty(x => x.Accessibility, b =>
            {
                b.Property(p => p.IsPublic);

                b.ComplexProperty(c => c.AuthKey, cb =>
                {
                    cb.Property(p => p.Value).HasColumnName("AuthKey");
                });
            });
        });
        base.OnModelCreating(modelBuilder);
    }
}