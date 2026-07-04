

namespace myshop.BLL.Services;

public class CategoryService(IUnitOfWork _unitOfWork)
{
    public async Task<IEnumerable<Category>> GetAlCategoriesAsync()
    {
        return await _unitOfWork.Repository<Category>()
                                .GetAllAsync(null);
    }

    public async Task CreateCategoryAsync(Category category)
    {
        //await _unitOfWork.Categories.AddAsync(category);
        //await _unitOfWork.SaveChangesAsync();
    }
}
