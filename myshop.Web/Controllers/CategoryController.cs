
namespace myshop.Web.Controllers;

public class CategoryController(ICategoryService _categoryService) : Controller
{
    public async Task<IActionResult> Index()
        => View();

    public async Task<IActionResult> GetData()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Json(new { data = categories });
    }


    public IActionResult Create()
        => View();


    [HttpPost]
    public async Task<IActionResult> Create(CategoryVM model)
    {
        if (ModelState.IsValid)
        {
            var isCreated = await _categoryService.CreateCategoryAsync(model);

            if (isCreated)
                TempData["Create"] = "Item has Created Successfully";
            else
                TempData["Create"] = "Data has not Updated Successfully";

            return RedirectToAction("Index");
        }
        return View(model);
    }




    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null | id == 0)
            NotFound();

        var model = await _categoryService.PrepareCategoryModelAsync(id!.Value);
        if (model is null)
            return NotFound();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoryVM model)
    {
        if (ModelState.IsValid)
        {
            var isUpdated = await _categoryService.UpdateCategoryAsync(model);

            if (isUpdated)
                TempData["Update"] = "Data has Updated Successfully";
            else
                TempData["Update"] = "Data has not Updated Successfully";

            return RedirectToAction("Index");
        }
        return View(model);
    }




    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null | id == 0)
            NotFound();

        var model = await _categoryService.PrepareCategoryModelAsync(id!.Value);
        if (model is null)
            return NotFound();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCategory(int? id)
    {
        var isDeleted = await _categoryService.DeleteCategoryAsync(id!.Value);

        if (isDeleted)
            TempData["Delete"] = "Item has Deleted Successfully";
        else
            TempData["Delete"] = "Item has not Deleted Successfully";
        
        return RedirectToAction("Index");
    }
}
