global using myshop.Domain.Interfaces;
using myshop.Domain.Entities;

namespace myshop.Application.Services;

public class CategoryService(IUnitOfWork _unitOfWork)
{
    public async Task<List<Category>>  GetAlCategoriesAsync()
    {
        //return await _unitOfWork.Categories.GetAllAsync();
        return null;
    }

    public async Task CreateCategoryAsync(Category category)
    {
        //await _unitOfWork.Categories.AddAsync(category);
        //await _unitOfWork.SaveChangesAsync();
    }
}
