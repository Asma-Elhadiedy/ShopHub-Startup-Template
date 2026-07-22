

namespace myshop.BLL.DependencyInjection;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddBLL()
        {
            services.AddScoped<ISeedData, SeedData>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFileService, FileService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            return services;
        }
    }
}
