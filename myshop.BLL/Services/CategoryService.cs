
namespace myshop.BLL.Services;

public class CategoryService(IUnitOfWork _unitOfWork, IMapper _mapper) : ICategoryService
{
    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        return await _unitOfWork.Repository<Category>()
                                .GetQueryable(null)
                                .Select(p => new CategoryDto()
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Description = p.Description
                                }).ToListAsync();
    }

    public async Task<CategoryVM?> PrepareCategoryModelAsync(int categoryId)
    {
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(categoryId);
        return category is not null
            ? _mapper.Map<CategoryVM>(category)
            : null;
    }

    public async Task<bool> CreateCategoryAsync(CategoryVM model)
    {
        var category = _mapper.Map<Category>(model);
        _unitOfWork.Repository<Category>().Add(category);

        if (await _unitOfWork.CompleteAsync() > 0)
            return true;
        return false;
    }

    public async Task<bool> UpdateCategoryAsync(CategoryVM model)
    {
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(model.Id);
        if (category is null)
            return false;
        
        //category = _mapper.Map(model, category);
        category.Name = model.Name;
        category.Description = model.Description;

        if (await _unitOfWork.CompleteAsync() > 0)
            return true;
        return false;
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(categoryId);
        if (category is null)
            return false;

        _unitOfWork.Repository<Category>().Remove(category);
        if (await _unitOfWork.CompleteAsync() > 0)
            return true;
        return false;
    }
}
