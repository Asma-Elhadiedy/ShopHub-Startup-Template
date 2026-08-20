

namespace myshop.BLL.Services.General;

public class SystemSettingsService(ILogger<LocalFileService> _logger, IUnitOfWork _unitOfWork) : ISystemSettingsService

{
    public async Task<string> GetStorageDomainPath(bool isWWWRoot)
    {
        try
        {            
            var domainPath = isWWWRoot
                ? string.Empty
                : await _unitOfWork.Repository<ApplicationSetting>()
                    .GetItemSelectedAsync(s => s.Key == ConstApplicationSettingsKeys.FileStoragePath, s => s.Value);
            
            return domainPath!;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return string.Empty;
        }
    }

}
