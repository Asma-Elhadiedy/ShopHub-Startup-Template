
namespace myshop.DAL.Configurations;

public class OrderStatusesConfigurations : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(EntityTypeBuilder<OrderStatus> builder)
    {
        builder.Property(p => p.Id).ValueGeneratedNever();
    }

}
