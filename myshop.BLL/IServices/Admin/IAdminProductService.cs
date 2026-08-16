
using myshop.BLL.DTOs.Admin;
using myshop.BLL.DTOs.General;

namespace myshop.BLL.IServices.Admin;

public interface IAdminProductService
{
    Task<PagingDTO<ProductDto>> GetAllProductsAsync(FormDto model, CancellationToken ct);
    Task<ProductVM?> PrepareProductModelAsync(int productId, CancellationToken ct);
    Task<bool> CreateProductAsync(ProductVM model, CancellationToken ct);
    Task<bool> UpdateProductAsync(ProductVM model, CancellationToken ct);
    Task<bool> DeleteProductAsync(int productId, CancellationToken ct);
    Task<IEnumerable<SelectListItem>> GetCategoriesSelectListAsync(CancellationToken ct);
}
