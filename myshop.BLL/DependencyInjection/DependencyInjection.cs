

namespace myshop.BLL.DependencyInjection;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddBLL(string connectionString)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFileService, FileService>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            services.AddDAL(connectionString);
            return services;
        }
    }
}
