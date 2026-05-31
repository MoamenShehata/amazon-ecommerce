using Amazon.Cart.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Amazon.Cart.Infrastructure.Configurations.Payments;

internal class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("methods", "payment");

        builder.HasIndex(x => x.Type).IsUnique();

        var cashMethod = PaymentMethod.ForCash();
        cashMethod.CreatedBy = "System";
        cashMethod.CreatedOn = new DateTime(2026, 05, 31);

        var visaMethod = PaymentMethod.ForVisa();
        visaMethod.CreatedBy = "System";
        visaMethod.CreatedOn = new DateTime(2026, 05, 31);

        //builder.HasData(cashMethod, visaMethod);
    }
}
