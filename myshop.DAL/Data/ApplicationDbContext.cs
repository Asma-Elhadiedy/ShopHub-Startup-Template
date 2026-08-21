

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


        var domainTypes = builder.Model.GetEntityTypes()
            .Where(t => typeof(DomainModelBase).IsAssignableFrom(t.ClrType));

        foreach (var entityType in domainTypes)
        {
            var type = entityType.ClrType;

            var param = Expression.Parameter(type, "e");
            var prop = Expression.Property(param, nameof(DomainModelBase.IsDeleted));
            var notDeleted = Expression.Lambda(Expression.Not(prop), param);
            
            builder.Entity(type).HasQueryFilter(notDeleted);
        }

        builder.ApplyConfigurationsFromAssembly(typeof(IDALMarker).Assembly);
    }

   
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderHeader> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<ShoppingCart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
    public DbSet<Review> Reviews { get; set; }


    public DbSet<OrderStatus> OrderStatuses { get; set; }
    public DbSet<CartStatus> CartStatuses { get; set; }
    public DbSet<PaymentStatus> PaymentStatuses { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }

}
