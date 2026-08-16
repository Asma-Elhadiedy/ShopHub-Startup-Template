
using myshop.BLL.DTOs.General;
using myshop.BLL.IServices.General;

namespace myshop.BLL.Services.Customer;

public class ProductService(
    ILogger<ProductService> _logger,
    IUnitOfWork _unitOfWork,
    ISystemSettingsService _settingsService,
    HybridCache _cache) : IProductService
{
    public async Task<IEnumerable<SelectListItem>> PrepareListAsync(CancellationToken ct)
    {
        try
        {
            var categories = await _cache.GetOrCreateAsync(
                ConstCacheCategories.SelectedList,
                async entry => await _unitOfWork.Repository<Category>()
                    .GetAllSelectedAsync(null,
                        p => new SelectListItem
                        {
                            Value = p.Id.ToString(),
                            Text = p.Name,
                        }, ct),
                tags: [ConstCacheCategories.Tag],
                cancellationToken: ct);

            var list = categories.ToList();

            list.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "All Categories"
            });
            return list;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }

    }

    public async Task<PagingDTO<ShoppingProductVM>> GetProductsByCategoryAsync(int categoryId, CancellationToken ct)
    {
        try
        {
            var storageDomainPath = await _settingsService.GetDomainPath(isWWWRoot: true);
            var predicate = categoryId == 0 ? null : (Expression<Func<Product, bool>>)(p => p.CategoryId == categoryId);

            var products = await _cache.GetOrCreateAsync(
                ConstCacheProducts.ByCategoryId(categoryId),
                async entry => await _unitOfWork.Repository<Product>()
                   .GetAllSelectedAsync(predicate,
                       p => new ShoppingProductVM
                       {
                           Id = p.Id,
                           Name = p.Name,
                           Price = p.Price,
                           ImagePath = BuildImageUrl(storageDomainPath, p.ImagePath),
                       }, ct),
                tags: [ConstCacheProducts.Tag],
                cancellationToken: ct);

            return new()
            {
                Data = products
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }

    }

    private static string BuildImageUrl(string domainPath, string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            return string.Empty;

        var normalized = imagePath.Replace('\\', '/').TrimStart('/');

        return $"/{normalized}";
    }
}
