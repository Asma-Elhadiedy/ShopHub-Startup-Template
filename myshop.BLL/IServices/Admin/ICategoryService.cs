
namespace myshop.BLL.IServices.Admin;

public interface ICategoryService
{
    Task<PagingDTO<CategoryDto>> GetAllCategoriesAsync(CancellationToken ct);
    Task<CategoryVM?> PrepareCategoryModelAsync(int categoryId, CancellationToken ct);
    Task<bool> CreateCategoryAsync(CategoryVM model, CancellationToken ct);
    Task<bool> UpdateCategoryAsync(CategoryVM model, CancellationToken ct);
    Task<bool> DeleteCategoryAsync(int categoryId, CancellationToken ct);
}
