

namespace myshop.BLL.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryVM?> PrepareCategoryModelAsync(int categoryId);
    Task<bool> CreateCategoryAsync(CategoryVM model);
    Task<bool> UpdateCategoryAsync(CategoryVM model);
    Task<bool> DeleteCategoryAsync(int categoryId);
}
