
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
            //categoryId = categoryId == 0 ? int.Parse((await PrepareListAsync(ct)).FirstOrDefault()?.Value ?? "1") : categoryId;
            var storageDomainPath = await _settingsService.GetStorageDomainPath(isWWWRoot: true);
            //var predicate = (Expression<Func<Product, bool>>)(p => p.CategoryId == categoryId);
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

    public async Task<ProductDetailsVM?> GetProductDetailsAsync(int productId, string? customerId, CancellationToken ct)
    {
        try
        {
            var storageDomainPath = await _settingsService.GetStorageDomainPath(isWWWRoot: true);
            var product = await _unitOfWork.Repository<Product>()
                .GetItemSelectedAsync(
                    item => item.Id == productId,
                    item => new ProductDetailsVM
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        ImagePath = BuildImageUrl(storageDomainPath, item.ImagePath),
                        Price = item.Price,
                        CategoryName = item.Category!.Name,
                        Reviews = item.Reviews!
                            .Select(review => new ProductReviewVM
                            {
                                Id = review.Id,
                                Rating = review.Rating,
                                Comment = review.Comment,
                                CustomerName = string.IsNullOrWhiteSpace(review.ApplicationUser!.FullName)
                                    ? "Customer"
                                    : review.ApplicationUser.FullName,
                                CreatedAt = review.CreatedAt,
                                IsMine = customerId != null && review.ApplicationUserId == customerId
                            })
                            .ToList()
                    }, ct);

            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve product details for product {ProductId} and customer {CustomerId}.", productId, customerId);
            throw;
        }
    }

    private static string BuildImageUrl(string domainPath, string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            return string.Empty;

        var normalized = imagePath.Replace('\\', '/').TrimStart('/');

        return $"{domainPath}/{normalized}";
    }
}
