using Amazon.Cart.Domain;
using Amazon.Cart.Domain.Payments.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Amazon.Cart.Infrastructure.Configurations.Payments;

internal class PaymentRequestConfiguration : IEntityTypeConfiguration<PayemntRequest>
{
    public void Configure(EntityTypeBuilder<PayemntRequest> builder)
    {
        builder.ToTable("requests", "payment");

        builder.ComplexProperty(c => c.Payload, b =>
        {
            b.Property(p => p.Payload).HasColumnName(nameof(PayemntRequest.Payload));
            b.Property(p => p.IsConfirmed).HasColumnName(nameof(PayemntRequest.IsConfirmed));
        });
    }
}
