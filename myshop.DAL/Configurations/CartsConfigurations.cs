
namespace myshop.DAL.Configurations;

public class CartsConfigurations : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
                
        builder.HasMany(x => x.CartItems)
            .WithOne(x => x.ShoppingCart)
            .HasForeignKey(x => x.CartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
