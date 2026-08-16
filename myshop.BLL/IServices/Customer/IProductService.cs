using myshop.BLL.DTOs.General;

namespace myshop.BLL.IServices.Customer;

public interface IProductService
{
    Task<IEnumerable<SelectListItem>> PrepareListAsync(CancellationToken ct);
    Task<PagingDTO<ShoppingProductVM>> GetProductsByCategoryAsync(int categoryId, CancellationToken ct);
}
