
namespace myshop.Web.Controllers;

public class CategoryController(ICategoryService _categoryService) : Controller
{
    public async Task<IActionResult> Index()
        => View();

    public async Task<IActionResult> GetData()
    {
        var data = await _categoryService.GetAllCategoriesAsync();
        return Json(new { data });
    }


    public IActionResult Create()
        => View();


    [HttpPost]
    public async Task<IActionResult> Create(CategoryVM model)
    {
        if (ModelState.IsValid)
        {
            var isCreated = await _categoryService.CreateCategoryAsync(model);


            TempData["Create"] = isCreated
                ? "Item has Created Successfully"
                : "Item has not Created Successfully";

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

            TempData["Update"] = isUpdated
                ? "Data has Updated Successfully"
                : "Data has not Updated Successfully";

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
    public async Task<IActionResult> Delete(int id)
    {
        var isDeleted = await _categoryService.DeleteCategoryAsync(id);

        TempData["Delete"] = isDeleted
            ? "Item has Deleted Successfully"
            : "Item has not Deleted Successfully";

        return RedirectToAction("Index");
    }
}
