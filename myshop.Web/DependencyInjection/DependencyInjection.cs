
namespace myshop.Web.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, string connectionString, string stripeKey)
    {
        services.AddDAL(connectionString).AddIdentityService().AddDefaultUI();
        services.AddBLL();

        services.AddControllersWithViews();
        services.AddRazorPages().AddRazorRuntimeCompilation();
        services.AddHttpContextAccessor();


        #region Auth & Policies  

        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        });

        services.AddAuthorizationCore(options =>
        {
            options.AddPolicy(ConstCustomPolicies.AdminAndTechnicalSupportRole,
                policy => policy.RequireRole(ConstRoles.Admin, ConstRoles.TechnicalSupport));
        });

        services.Configure<SecurityStampValidatorOptions>(options =>
        {
            options.ValidationInterval = TimeSpan.Zero;
        });

        services.ConfigureApplicationCookie(cfg =>
        {
            cfg.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            cfg.LogoutPath = "/Home/Index";
            cfg.LoginPath = "/Account/Login";
            cfg.AccessDeniedPath = "/Account/Login";
        });
        #endregion


        #region Session
        services.AddDistributedMemoryCache();
        services.AddHybridCache(options => options.DefaultEntryOptions = new()
        {
            Expiration = TimeSpan.FromMinutes(30),
            LocalCacheExpiration = TimeSpan.FromMinutes(30)
        });

        services.AddSession(cfg => cfg.IdleTimeout = TimeSpan.FromMinutes(30));

        #endregion


        #region mini-profiler
        services.AddMiniProfiler(options =>
        {
            options.RouteBasePath = "/profiler";
            options.PopupRenderPosition = StackExchange.Profiling.RenderPosition.BottomLeft;
            options.PopupShowTimeWithChildren = true;
        }).AddEntityFramework();
        #endregion

        #region Payment
        services.AddSingleton(new StripeClient(stripeKey));
        #endregion

        return services;
    }

}