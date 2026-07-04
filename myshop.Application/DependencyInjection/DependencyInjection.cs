

namespace myshop.Application.DependencyInjection;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            //services.AddAutoMapper(typeof(MappingProfie));
            return services;
        }
    }
}
