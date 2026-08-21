
namespace myshop.DAL.Configurations;

public class CartStatusesConfigurations : IEntityTypeConfiguration<CartStatus>
{
    public void Configure(EntityTypeBuilder<CartStatus> builder)
    {
        builder.Property(p => p.Id).ValueGeneratedNever();
    }

}
