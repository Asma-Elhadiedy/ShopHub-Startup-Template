
namespace myshop.BLL.DependencyInjection;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddBLL()
        {

            services.AddAdminServices();
            services.AddCustomerServices();


            services.AddScoped<SmtpClient>();
            services.AddScoped<IEmailSenderService, MailKitSenderService>();
            services.AddOptions<MailKitOptions>()
                .BindConfiguration(MailKitOptions.MailKit)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddScoped<ISeedData, SeedData>();
            services.AddScoped<IFileService, LocalFileService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISystemSettingsService, SystemSettingsService>();

            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            return services;
        }
        public IServiceCollection AddAdminServices()
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAdminOrderService, AdminOrderService>();
            services.AddScoped<IAdminProductService, AdminProductService>();
            services.AddScoped<IAdminSettingsService, AdminSettingsService>();

            return services;
        }
        public IServiceCollection AddCustomerServices()
        {
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
