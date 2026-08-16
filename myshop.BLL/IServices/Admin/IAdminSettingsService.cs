
namespace myshop.BLL.IServices.Admin;

public interface IAdminSettingsService
{
    Task<int> RestoreDeletedProductsAsync(string adminId);
}

