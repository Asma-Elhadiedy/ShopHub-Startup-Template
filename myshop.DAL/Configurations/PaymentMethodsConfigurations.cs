
namespace myshop.DAL.Configurations;

public class PaymentMethodsConfigurations : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.Property(p => p.Id).ValueGeneratedNever();
    }

}
