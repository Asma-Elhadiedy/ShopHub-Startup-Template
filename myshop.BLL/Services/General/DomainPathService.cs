

namespace myshop.BLL.Services.General;

public class DomainPathService(IHttpContextAccessor _httpContext)
{
    private string? _domainPath;
    public string GetDomainPath()
    {
        if (_domainPath is null)
        {
            var request = _httpContext.HttpContext.Request;
            _domainPath = $"{request.Scheme}://{request.Host}{request.PathBase}";
        }
        return _domainPath;
    }
}