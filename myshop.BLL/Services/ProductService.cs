

using Microsoft.Extensions.Hosting;

namespace myshop.BLL.Services;

public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper, IHostEnvironment _webHostEnvironment) : IProductService
{
    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        return await _unitOfWork.Repository<Product>()
                                .GetQueryable(null)
                                .Select(p => new ProductDto()
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Description = p.Description,
                                    Price = p.Price,
                                    CategoryName = p.Category.Name
                                }).ToListAsync();
    }

    public async Task<ProductVM?> PrepareProductModelAsync(int productId)
    {
        var category = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
        return category is not null
            ? _mapper.Map<ProductVM>(category)
            : null;
    }

    public async Task<bool> CreateProductAsync(ProductVM model)
    {
        var category = _mapper.Map<Category>(model);
        await _unitOfWork.Repository<Category>().AddAsync(category);

        if (await _unitOfWork.CompleteAsync() > 0)
            return true;
        return false;
    }

    public async Task<bool> UpdateProductAsync(ProductVM model)
    {
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(model.Product.Id);
        category = _mapper.Map(model, category);

        if (await _unitOfWork.CompleteAsync() > 0)
            return true;
        return false;
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
        if (product is null)
            return false;


        var oldimg = Path.Combine(_webHostEnvironment.ContentRootPath, product.Img.TrimStart('\\'));
        if (File.Exists(oldimg))
            File.Delete(oldimg);


        _unitOfWork.Repository<Product>().Remove(product);
        if (await _unitOfWork.CompleteAsync() > 0)
            return true;
        return false;
    }
}
