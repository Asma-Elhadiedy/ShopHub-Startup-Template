namespace myshop.BLL.IServices.General;

public interface ISystemSettingsService
{
    Task<string> GetStorageDomainPath(bool isWWWRoot);
}

