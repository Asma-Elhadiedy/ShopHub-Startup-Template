namespace myshop.BLL.IServices.General;

public interface ISystemSettingsService
{
    Task<string> GetDomainPath(bool isWWWRoot);
}

