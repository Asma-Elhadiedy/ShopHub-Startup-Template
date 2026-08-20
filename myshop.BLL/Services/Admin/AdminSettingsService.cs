
namespace myshop.BLL.Services.Admin;

public class AdminSettingsService(
    ILogger<AdminSettingsService> _logger,
    IUnitOfWork _unitOfWork) : IAdminSettingsService
{
    public async Task<int> RestoreDeletedProductsAsync(string adminId)
    {
        try
        {
            var updatedCount = await _unitOfWork.Repository<Product>()
                .BulkUpdateAsync(
                    p => p.IsDeleted == true,
                    setters => setters.SetProperty(p => p.IsDeleted, false).SetProperty(p => p.DeletedAt, p => null),
                    ignoreQueryFilters: true);

            _logger.LogWarning("Restored {UpdatedCount} deleted products by admin with Id: {AdminId}.", updatedCount, adminId);
            return updatedCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to restore deleted products by admin with Id: {AdminId}.", adminId);
            throw;
        }
    }
}
