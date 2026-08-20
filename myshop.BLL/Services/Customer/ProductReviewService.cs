using myshop.BLL.ViewModels.Customer.Products;

namespace myshop.BLL.Services.Customer;

public class ProductReviewService(
    ILogger<ProductReviewService> _logger,
    IUnitOfWork _unitOfWork) : IProductReviewService
{
    public async Task<bool> HasReviewAsync(int productId, string customerId, CancellationToken ct)
    {
        try
        {
            return await _unitOfWork.Repository<Review>()
                .IsExistAsync(review => review.ProductId == productId && review.ApplicationUserId == customerId, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check review existence for product {ProductId} and customer {CustomerId}.", productId, customerId);
            throw;
        }
    }

    public async Task<int?> CreateAsync(ProductReviewFormVM model, string customerId, CancellationToken ct)
    {
        try
        {
            if (!IsValid(model) || string.IsNullOrWhiteSpace(customerId))
                return null;

            var productExists = await _unitOfWork.Repository<Product>()
                .IsExistAsync(product => product.Id == model.ProductId, ct);
            if (!productExists || await HasReviewAsync(model.ProductId, customerId, ct))
                return null;

            var hasPurchased = await _unitOfWork.Repository<OrderItem>()
               .IsExistAsync(i => i.ProductId == model.ProductId && i.Order!.ApplicationUserId == customerId, ct);
            if (!hasPurchased)
                return 0;

            var review = new Review
            {
                ProductId = model.ProductId,
                ApplicationUserId = customerId,
                Rating = model.Rating,
                Comment = model.Comment.Trim()
            };

            _unitOfWork.Repository<Review>().Add(review);
            await _unitOfWork.CompleteAsync(ct);
            return review.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create review for product {ProductId} and customer {CustomerId}.", model.ProductId, customerId);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(ProductReviewFormVM model, string customerId, CancellationToken ct)
    {
        try
        {
            if (!IsValid(model) || string.IsNullOrWhiteSpace(customerId))
                return false;

            var review = await _unitOfWork.Repository<Review>()
                .GetItemAsync(item => item.Id == model.ReviewId && item.ProductId == model.ProductId && item.ApplicationUserId == customerId, ct);
            if (review is null)
                return false;

            review.Rating = model.Rating;
            review.Comment = model.Comment.Trim();
            await _unitOfWork.CompleteAsync(ct);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update review {ReviewId} for product {ProductId} and customer {CustomerId}.", model.ReviewId, model.ProductId, customerId);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int reviewId, string customerId, CancellationToken ct)
    {
        try
        {
            if (reviewId <= 0 || string.IsNullOrWhiteSpace(customerId))
                return false;

            var review = await _unitOfWork.Repository<Review>()
                .GetItemAsync(item => item.Id == reviewId && item.ApplicationUserId == customerId, ct);
            if (review is null)
                return false;

            _unitOfWork.Repository<Review>().Remove(review);
            await _unitOfWork.CompleteAsync(ct);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete review {ReviewId} for customer {CustomerId}.", reviewId, customerId);
            throw;
        }
    }

    private static bool IsValid(ProductReviewFormVM model)
        => model.Rating is >= 1 and <= 5
            && !string.IsNullOrWhiteSpace(model.Comment)
            && model.Comment.Trim().Length is >= 3 and <= 4000;
}
