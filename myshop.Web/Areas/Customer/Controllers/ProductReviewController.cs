namespace myshop.Web.Areas.Customer.Controllers;

[Authorize]
[Area("Customer")]
public class ProductReviewController(
    ILogger<ProductReviewController> _logger,
    IProductReviewService _reviewService) : Controller
{
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductReviewFormVM model, CancellationToken ct)
    {
        if (model.ProductId <= 0)
            return RedirectToAction("Index", "Home", new { area = "Customer" });

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please choose a rating and write a review before submitting.";
            return RedirectToAction("Details", "Product", new { area = "Customer", id = model.ProductId });
        }

        try
        {
            var reviewId = await _reviewService.CreateAsync(model, User.Id!, ct);
            if (!reviewId.HasValue)
            {
                TempData["Error"] = "You have already reviewed this product, or the product is no longer available.";
                return RedirectToAction("Details", "Product", new { area = "Customer", id = model.ProductId });
            }
            if (reviewId == 0)
            {
                TempData["Error"] = "You should try it first 😁!";
                return RedirectToAction("Details", "Product", new { area = "Customer", id = model.ProductId });
            }

            TempData["Success"] = "Your review was added successfully.";
            return RedirectToAction("Details", "Product", new { area = "Customer", id = model.ProductId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create review for product {ProductId} by customer {CustomerId}.", model.ProductId, User.Id);
            TempData["Error"] = "We could not save your review. Please try again.";
            return RedirectToAction("Details", "Product", new { area = "Customer", id = model.ProductId });
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductReviewFormVM model, CancellationToken ct)
    {
        if (model.ProductId <= 0)
            return RedirectToAction("Index", "Home", new { area = "Customer" });

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please choose a rating and write a review before saving.";
            return RedirectToAction("Details", "Product", new { area = "Customer", id = model.ProductId });
        }

        try
        {
            var updated = await _reviewService.UpdateAsync(model, User.Id!, ct);
            TempData[updated ? "Success" : "Error"] = updated
                ? "Your review was updated successfully."
                : "That review could not be found or edited.";
            return RedirectToAction("Details", "Product", new { area = "Customer", id = model.ProductId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update review {ReviewId} for product {ProductId} by customer {CustomerId}.", model.ReviewId, model.ProductId, User.Id);
            TempData["Error"] = "That review could not be found or edited.";
            return RedirectToAction("Details", "Product", new { area = "Customer", id = model.ProductId });
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int reviewId, int productId, CancellationToken ct)
    {
        if (productId <= 0)
            return RedirectToAction("Index", "Home", new { area = "Customer" });

        try
        {
            var deleted = await _reviewService.DeleteAsync(reviewId, User.Id!, ct);
            TempData[deleted ? "Success" : "Error"] = deleted
                ? "Your review was deleted."
                : "That review could not be found or deleted.";
            return RedirectToAction("Details", "Product", new { area = "Customer", id = productId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete review {ReviewId} for product {ProductId} by customer {CustomerId}.", reviewId, productId, User.Id);
            TempData["Error"] = "That review could not be found or deleted.";
            return RedirectToAction("Details", "Product", new { area = "Customer", id = productId });
        }
    }
}
