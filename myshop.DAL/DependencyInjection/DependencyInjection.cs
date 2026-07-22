
namespace myshop.DAL.DependencyInjection;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDAL(string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString));

            return services;
        }

        public IdentityBuilder AddIdentityService()
        {
            return services.AddIdentity<ApplicationUser, IdentityRole>(
                 options => options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(4))
                 .AddDefaultTokenProviders()
                 .AddEntityFrameworkStores<ApplicationDbContext>();

        }
    }
}
