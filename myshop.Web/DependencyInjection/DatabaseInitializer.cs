
namespace myshop.Web.DependencyInjection;

internal static class DatabaseInitializer 
{
    internal static async Task<IApplicationBuilder> InitializeDatabaseAsync(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();
        var seedData = scope.ServiceProvider.GetRequiredService<ISeedData>();
        await seedData.SeedAsync();

        return builder;
    }
}
