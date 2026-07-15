

namespace myshop.BLL.DBInitializer;

public class SeedData(RoleManager<IdentityRole> _roleManager) : ISeedData
{
    public async Task SeedAsync()
    {
        try
        {
            await SeedDefaultRoles();
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private async Task SeedDefaultRoles()
    {
        if (await _roleManager.Roles.AnyAsync())
            return;

        await _roleManager.CreateAsync(new IdentityRole(ConstRoles.Admin));
        await _roleManager.CreateAsync(new IdentityRole(ConstRoles.Customer));
    }
}
