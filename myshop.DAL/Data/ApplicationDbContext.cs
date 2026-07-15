

namespace myshop.DAL.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    base.OnModelCreating(builder);

    //    builder.ApplyConfigurationsFromAssembly(typeof(IDALMarker).Assembly);
    //}

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }


}
