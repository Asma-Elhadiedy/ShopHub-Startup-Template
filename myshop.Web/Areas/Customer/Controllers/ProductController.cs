namespace myshop.Web.Areas.Customer.Controllers;

[Area("Customer")]
public class ProductController(ILogger<ProductController> _logger, IProductService _productService) : Controller
{
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        if (id <= 0)
            return BadRequest();

        try
        {
            var customerId = User.Identity?.IsAuthenticated == true ? User.Id : null;
            var product = await _productService.GetProductDetailsAsync(id, customerId, ct);
            if (product is null)
                return NotFound();

            return View(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve product details for product {ProductId} and customer {CustomerId}.", id, User.Id);
            return BadRequest();
        }
    }
}
