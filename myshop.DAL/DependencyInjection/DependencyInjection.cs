
namespace myshop.DAL.DependencyInjection;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDAL()
        {

            services.AddIdentity<ApplicationUser, IdentityRole>(
                options => options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(4)
                ).AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }
    }
}
