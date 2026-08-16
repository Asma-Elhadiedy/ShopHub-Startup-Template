

using Microsoft.Extensions.Options;

namespace myshop.DAL.DependencyInjection;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDAL(string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public IdentityBuilder AddIdentityService()
        {
            return services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                 {
                     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(4);
                     options.Password.RequireNonAlphanumeric = false;
                     options.Password.RequireDigit = false;
                     options.Password.RequireUppercase = false;
                     options.Password.RequireLowercase = false;
                 })
                 .AddDefaultTokenProviders()
                 .AddEntityFrameworkStores<ApplicationDbContext>();

        }
    }
}
