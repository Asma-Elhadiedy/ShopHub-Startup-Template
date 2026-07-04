global using Microsoft.Extensions.DependencyInjection;
global using myshop.Infrastructure.Repositories;
global using Microsoft.AspNetCore.Identity;
using myshop.Application.DependencyInjection;

namespace myshop.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure()
        {

            services.AddIdentity<ApplicationUser, IdentityRole>(
                options => options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(4)
                ).AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddApplication();
            return services;
        }
    }
}
