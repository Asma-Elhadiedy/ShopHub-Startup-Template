
namespace myshop.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = ConstRoles.Admin)]
public class ProductController(ILogger<ProductController> _logger, IAdminProductService _productService) : Controller
{
    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> GetData(CancellationToken ct)
    {
        var model = Request.Form.GetRequestForm();
        try
        {
            var productsDT = await _productService.GetAllProductsAsync(model, ct);
            return Json(productsDT);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get products for datatable with model: {model}", model);
            return BadRequest(new ResponseMessageDto
            {
                Title = ConstMessages.ErrorTitle,
                Message = ConstMessages.ErrorMessage
            });
        }
    }



    public async Task<IActionResult> Create(CancellationToken ct)
    {
        try
        {
            var model = await _productService.PrepareProductModelAsync(0, ct);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to prepare product creation model.");
            TempData["Error"] = ConstMessages.ErrorFetchingDataMessage;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductVM model, CancellationToken ct)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                model.CategoryList = await _productService.GetCategoriesSelectListAsync(ct);
                return View(model);
            }

            var isCreated = await _productService.CreateProductAsync(model, ct);
            if (isCreated)
                TempData["Success"] = ConstMessages.SuccessCreatedMessage;
            else
                TempData["Error"] = ConstMessages.ErrorCreatingMessage;

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create product with model: {model}", model);
            TempData["Error"] = ConstMessages.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }
    }




    public async Task<IActionResult> Edit(int? id, CancellationToken ct)
    {
        try
        {
            if (id == null || id == 0)
                return NotFound();

            var model = await _productService.PrepareProductModelAsync(id!.Value, ct);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get product data with the id: {productId}, to update", id);
            TempData["Error"] = ConstMessages.ErrorFetchingDataMessage;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductVM model, CancellationToken ct)
    {
        try
        {
            ModelState.Remove(nameof(model.File));
            if (!ModelState.IsValid)
            {
                model.CategoryList = await _productService.GetCategoriesSelectListAsync(ct);
                return View(model);
            }

            var isUpdated = await _productService.UpdateProductAsync(model, ct);
            if (isUpdated)
                TempData["Success"] = ConstMessages.SuccessUpdatedMessage;
            else
                TempData["Error"] = ConstMessages.ErrorUpdatingMessage;

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update product data with {model}", model);
            TempData["Error"] = ConstMessages.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }
    }



    public async Task<IActionResult> Delete(int? id, CancellationToken ct)
    {
        try
        {
            if (id == null || id == 0)
                return NotFound();

            var model = await _productService.PrepareProductModelAsync(id!.Value, ct);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get product data with id: {productId}, to delete", id);
            TempData["Error"] = ConstMessages.ErrorFetchingDataMessage;
            return RedirectToAction(nameof(Index));
        }
    }


    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        try
        {
            var isDeleted = await _productService.DeleteProductAsync(id, ct);
            if (isDeleted)
                TempData["Success"] = ConstMessages.SuccessDeletedMessage;
            else
                TempData["Error"] = ConstMessages.ErrorDeletingMessage;

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete product with the id: {productId}", id);
            TempData["Error"] = ConstMessages.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }
    }


    public async Task<IActionResult> GetCategoriesSelectListAsync(CancellationToken ct)
        => Json(await _productService.GetCategoriesSelectListAsync(ct));
}
