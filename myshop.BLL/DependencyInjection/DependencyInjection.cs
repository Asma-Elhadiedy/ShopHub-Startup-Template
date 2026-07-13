

namespace myshop.BLL.DependencyInjection;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddBLL()
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfie>());

            services.AddDAL();
            return services;
        }
    }
}
