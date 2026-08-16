

using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace myshop.DAL.Data;

public class ApplicationDbContext : IdentityDbContext<
    ApplicationUser,
    ApplicationRole,
    string,
    IdentityUserClaim<string>,
    ApplicationUserRole,
    IdentityUserLogin<string>,
    IdentityRoleClaim<string>,
    IdentityUserToken<string>>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);

        builder.ApplyConfigurationsFromAssembly(typeof(IDALMarker).Assembly);
    }

   
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderHeader> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<ShoppingCart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<ApplicationSetting> ApplicationSettings { get; set; }

}
