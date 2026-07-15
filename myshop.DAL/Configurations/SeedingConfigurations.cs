
namespace myshop.DAL.Configurations;

public class SeedingConfigurations : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        //builder.HasData(
        //    new IdentityRole(ConstRoles.Admin),
        //    new IdentityRole(ConstRoles.Customer)
        //);
    }
   
}
