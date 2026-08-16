
namespace myshop.BLL.Services.Admin;

public class AdminProductService(
    ILogger<AdminProductService> _logger,
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    IFileService _fileService,
    HybridCache _cache) : IAdminProductService
{
    public async Task<PagingDTO<ProductDto>> GetAllProductsAsync(FormDto model, CancellationToken ct)
    {
        var productsQuery = _unitOfWork.Repository<Product>()
            .GetQueryable(null)
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider);

        if (!string.IsNullOrEmpty(model.Search?.Trim()))
            productsQuery = productsQuery
                .Where(p => p.Name.Contains(model.Search) ||
                            p.Description.Contains(model.Search));

        if (model.SortingCol is not null && model.SortingDir is not null)
            productsQuery = productsQuery.OrderBy($"{model.SortingCol} {model.SortingDir}");

        var recordsTotal = productsQuery.Count();
        var pagedProducts = await productsQuery
               .Skip(model.Start)
               .Take(model.PageSize)
               .ToListAsync(ct);

        return new()
        {
            RecordsTotal = recordsTotal,
            RecordsFiltered = recordsTotal,
            Data = pagedProducts
        };
    }


    public async Task<ProductVM?> PrepareProductModelAsync(int productId, CancellationToken ct)
    {
        try
        {
            var product = productId == 0
                ? null
                : await _unitOfWork.Repository<Product>().GetByIdAsync(productId, ct);

            var productVM = new ProductVM { CategoryList = await GetCategoriesSelectListAsync(ct) };
            if (product != null)
                _mapper.Map(product, productVM);

            return productVM;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new();
        }
    }

    public async Task<bool> CreateProductAsync(ProductVM model, CancellationToken ct)
    {
        string? filePath = null;
        try
        {
            var product = _mapper.Map<Product>(model);
            _unitOfWork.Repository<Product>().Add(product);

            if (model.File != null)
            {
                filePath = await _fileService.SaveFileAsync(model.File, ConstPath.ProductImagesPath);
                product.ImagePath = filePath;
            }
            if (product.ImagePath is not null && await _unitOfWork.CompleteAsync(ct) > 0)
            {
                await RemoveCachedProducts();
                return true;
            }
            if (product.ImagePath is not null)
                _fileService.DeleteFile(product.ImagePath);

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            if (filePath is not null)
                _fileService.DeleteFile(filePath);
            throw;
        }

    }

    public async Task<bool> UpdateProductAsync(ProductVM model, CancellationToken ct)
    {
        var isUpdateingImage = model.File != null;
        string? newImagePath = null;
        try
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(model.Id, ct);
            if (product is null)
            {
                _logger.LogError("Couldn't find product with id: {id}", model.Id);
                return false;
            }

            var oldimgPath = product.ImagePath;
            product = _mapper.Map(model, product);

            var isChanged = _unitOfWork.Repository<Product>().IsItemChanged();
            if (!isChanged && !isUpdateingImage)
                return true;

            if (isUpdateingImage)
            {
                newImagePath = await _fileService.SaveFileAsync(model.File, ConstPath.ProductImagesPath);
                if (!string.IsNullOrEmpty(newImagePath))
                    product.ImagePath = newImagePath;
                else
                    isUpdateingImage = false;
            }

            if (await _unitOfWork.CompleteAsync(ct) > 0)
            {
                if (isUpdateingImage)
                    _fileService.DeleteFile(oldimgPath);
                await RemoveCachedProducts();
                return true;
            }

            if (isUpdateingImage)
                _fileService.DeleteFile(newImagePath!);
            return false;
        }
        catch (Exception ex)
        {
            if (isUpdateingImage)
                _fileService.DeleteFile(newImagePath!);

            _logger.LogError(ex, ex.Message);
            return false;
        }
    }

    public async Task<bool> DeleteProductAsync(int productId, CancellationToken ct)
    {
        try
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId, ct);
            if (product is null)
                return false;

            _unitOfWork.Repository<Product>().Remove(product);

            if (await _unitOfWork.CompleteAsync(ct) > 0)
            {
                var oldimg = Path.Combine(product.ImagePath);
                _fileService.DeleteFile(oldimg);
                await RemoveCachedProducts();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<SelectListItem>> GetCategoriesSelectListAsync(CancellationToken ct)
    {
        try
        {
            var categories = await _cache.GetOrCreateAsync(
                ConstCacheCategories.SelectedList,
                async entry => await _unitOfWork.Repository<Category>()
                    .GetAllSelectedAsync(null,
                        c => new SelectListItem()
                        {
                            Text = c.Name,
                            Value = c.Id.ToString()
                        }, ct),
                tags: [ConstCacheCategories.Tag],
                cancellationToken: ct);

            return categories;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return [];
        }
    }

    async Task RemoveCachedProducts()
    {
        await _cache.RemoveByTagAsync(ConstCacheProducts.Tag);
    }
}
