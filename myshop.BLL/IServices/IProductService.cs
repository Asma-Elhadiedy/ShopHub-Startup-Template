
namespace myshop.BLL.IServices;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductVM?> PrepareProductModelAsync(int productId);
    Task<bool> CreateProductAsync(ProductVM model);
    Task<bool> UpdateProductAsync(ProductVM model);
    Task<bool> DeleteProductAsync(int productId);
}
