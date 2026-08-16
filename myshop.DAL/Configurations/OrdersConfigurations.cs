
namespace myshop.DAL.Configurations;

public class OrdersConfigurations : IEntityTypeConfiguration<OrderHeader>
{
    public void Configure(EntityTypeBuilder<OrderHeader> builder)
    {
        builder.Property(p => p.TotalPrice)
            .HasComputedColumnSql("[Subtotal] + [ShippingCost]");

        builder.HasMany(x => x.OrderItems)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
