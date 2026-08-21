
namespace myshop.DAL.Configurations;

public class PaymentStatusesConfigurations : IEntityTypeConfiguration<PaymentStatus>
{
    public void Configure(EntityTypeBuilder<PaymentStatus> builder)
    {
        builder.Property(p => p.Id).ValueGeneratedNever();
    }

}
