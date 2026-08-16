
namespace myshop.Web.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController(ILogger<HomeController> _logger, IProductService _productService, ICartService _cartService) : Controller
{

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        try
        {
            var categories = await _productService.PrepareListAsync(ct);
            if (TempData["BeforeLoginCartId"] is int beforeLoginCartId)
            {
                //User has cart pre-login
                //Handle the case where the user has another active cart from previous login
                var oldCart = await _cartService.GetCartDataAsync(User.Id, HttpContext.Session.Id, ct);
                if (oldCart != null && oldCart.Id != beforeLoginCartId)
                    // Ask in client side if the user wants to restore old cart
                    ViewData["BeforeLoginCartId"] = beforeLoginCartId;
            }

            return View(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to prepare home page for shopping.");
            return RedirectToAction("Error");
        }
    }

    public async Task<IActionResult> GetProducts(int id, CancellationToken ct)
    {
        try
        {
            var products = await _productService.GetProductsByCategoryAsync(id, ct);
            return PartialView("_ProductsSection", products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get products with category id: {categoryId}", id);
            return BadRequest(new ResponseMessageDto
            {
                Title = ConstMessages.ErrorTitle,
                Message = ConstMessages.ErrorMessage
            });
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}