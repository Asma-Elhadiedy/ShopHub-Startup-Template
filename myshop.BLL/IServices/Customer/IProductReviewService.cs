using myshop.BLL.ViewModels.Customer.Products;

namespace myshop.BLL.IServices.Customer;

public interface IProductReviewService
{
    Task<bool> HasReviewAsync(int productId, string customerId, CancellationToken ct);
    Task<int?> CreateAsync(ProductReviewFormVM model, string customerId, CancellationToken ct);
    Task<bool> UpdateAsync(ProductReviewFormVM model, string customerId, CancellationToken ct);
    Task<bool> DeleteAsync(int reviewId, string customerId, CancellationToken ct);
}
