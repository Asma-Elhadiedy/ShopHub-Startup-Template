
namespace myshop.BLL.Services.Admin;

public class CategoryService(
    ILogger<CategoryService> _logger,
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    HybridCache _cache) : ICategoryService
{
    public async Task<PagingDTO<CategoryDto>> GetAllCategoriesAsync(CancellationToken ct)
    {
        try
        {
            var categoriesQuery = await _cache.GetOrCreateAsync(
                ConstCacheCategories.All,
                async entry => await _unitOfWork.Repository<Category>()
                                    .GetQueryable(null)
                                    .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                                    .ToListAsync(ct),
                tags: [ConstCacheCategories.Tag],
                cancellationToken: ct);

            return new()
            {
                RecordsTotal = categoriesQuery.Count,
                RecordsFiltered = categoriesQuery.Count,
                Data = categoriesQuery
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving categories.");
            throw;
        }
    }

    public async Task<CategoryVM?> PrepareCategoryModelAsync(int categoryId, CancellationToken ct)
    {
        try
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(categoryId, ct);
            return category is not null
                ? _mapper.Map<CategoryVM>(category)
                : new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<bool> CreateCategoryAsync(CategoryVM model, CancellationToken ct)
    {
        try
        {
            var category = _mapper.Map<Category>(model);
            _unitOfWork.Repository<Category>().Add(category);

            if (await _unitOfWork.CompleteAsync(ct) > 0)
            {
                await RemoveCachedCategories();
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

    public async Task<bool> UpdateCategoryAsync(CategoryVM model, CancellationToken ct)
    {
        try
        {
            var category = await _unitOfWork.Repository<Category>()
                .GetByIdAsync(model.Id, ct);

            if (category is null)
                return false;

            category.Name = model.Name;
            category.Description = model.Description;

            if (!_unitOfWork.Repository<Category>().IsItemChanged())
                return true;

            if (await _unitOfWork.CompleteAsync(ct) > 0)
            {
                await RemoveCachedCategories();
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

    public async Task<bool> DeleteCategoryAsync(int categoryId, CancellationToken ct)
    {
        try
        {
            var category = await _unitOfWork.Repository<Category>()
                .GetByIdAsync(categoryId, ct);

            if (category is null)
                return false;

            _unitOfWork.Repository<Category>().Remove(category);
            if (await _unitOfWork.CompleteAsync(ct) > 0)
            {
                await RemoveCachedCategories();
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

    async Task RemoveCachedCategories()
    {
        await _cache.RemoveByTagAsync(ConstCacheCategories.Tag);
    }
}
