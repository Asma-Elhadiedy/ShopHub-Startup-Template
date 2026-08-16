
namespace myshop.Web.Areas.Admin.Controllers;


[Area("Admin")]
[Authorize(ConstCustomPolicies.AdminAndTechnicalSupportRole)]
public class CategoryController(ILogger<CategoryController> _logger, ICategoryService _categoryService) : Controller
{
    public IActionResult Index() => View();

    public async Task<IActionResult> GetData(CancellationToken ct)
    {
        try
        {
            var categoriesDT = await _categoryService.GetAllCategoriesAsync(ct);
            return Json(categoriesDT);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get categories for datatable.");
            return BadRequest(new ResponseMessageDto
            {
                Title = ConstMessages.ErrorTitle,
                Message = ConstMessages.ErrorMessage
            });
        }
    }


    public IActionResult Create()
        => View();


    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryVM model, CancellationToken ct)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var isCreated = await _categoryService.CreateCategoryAsync(model, ct);

                if (isCreated)
                    TempData["Success"] = ConstMessages.SuccessCreatedMessage;
                else
                    TempData["Error"] = ConstMessages.ErrorCreatingMessage;

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create category with data: {model}", model);
            TempData["Error"] = ConstMessages.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }
    }




    public async Task<IActionResult> Edit(int? id, CancellationToken ct)
    {
        try
        {
            if (id == null | id == 0)
                NotFound();

            var model = await _categoryService.PrepareCategoryModelAsync(id!.Value, ct);
            if (model is null)
                return NotFound();
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get category to edit: {categoryId}", id);
            TempData["Error"] = ConstMessages.ErrorFetchingDataMessage;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryVM model, CancellationToken ct)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(model);

            var isUpdated = await _categoryService.UpdateCategoryAsync(model, ct);
            if (isUpdated)
                TempData["Success"] = ConstMessages.SuccessUpdatedMessage;
            else
                TempData["Error"] = ConstMessages.ErrorUpdatingMessage;

            return RedirectToAction(nameof(Index));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update category with model: {model}", model);
            TempData["Error"] = ConstMessages.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }
    }




    public async Task<IActionResult> Delete(int? id, CancellationToken ct)
    {
        try
        {
            if (id == null || id == 0)
                NotFound();

            var model = await _categoryService.PrepareCategoryModelAsync(id!.Value, ct);
            if (model is null)
                return NotFound();
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get category with id: {categoryId}, to delete", id);
            TempData["Error"] = ConstMessages.ErrorFetchingDataMessage;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        try
        {
            if (id == 0)
                NotFound();

            var isDeleted = await _categoryService.DeleteCategoryAsync(id, ct);

            if (isDeleted)
                TempData["Success"] = ConstMessages.SuccessDeletedMessage;
            else
                TempData["Error"] = ConstMessages.ErrorDeletingMessage;

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete category with id: {categoryId}", id);
            TempData["Error"] = ConstMessages.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }
    }
}
