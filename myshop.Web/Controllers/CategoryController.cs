

using myshop.Application.Services;

namespace myshop.Web.Controllers;

public class CategoryController(CategoryService _categoryService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAlCategoriesAsync();
        return View(categories);
    }

    [HttpGet]
    public IActionResult Create()
    {

        return View();
    }
    
    [HttpPost]
    public IActionResult Create(Category category)
    {
        if (ModelState.IsValid)
        {
            _categoryService.CreateCategoryAsync(category);
            TempData["Create"] = "Item has Created Successfully";
            return RedirectToAction("Index");
        }
        return View(category);
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null | id == 0)
        {
            NotFound();
        }
        //var categoryIndb = _categoryService.Find(id);
        Category categoryIndb = new();
        return View(categoryIndb);
    }

    [HttpPost]
    public IActionResult Edit(Category category)
    {
        if (ModelState.IsValid)
        {
            //_categoryService.Categories.Update(category);

            //_categoryService.SaveChanges();
            TempData["Update"] = "Data has Updated Successfully";
            return RedirectToAction("Index");
        }
        return View(category);
    }

    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null | id == 0)
        {
            NotFound();
        }
        //var categoryIndb = _categoryService.Categories.Where(x => x.Id == id).FirstOrDefault();

        Category category = new();
        return View(category);
    }

    [HttpPost]
    public IActionResult DeleteCategory(int? id)
    {
        //var categoryIndb = _categoryService.CreateCategoryAsync() ;
        //if (categoryIndb == null)
        //{
        //    NotFound();
        //}
        //_categoryService.CreateCategoryAsync(categoryIndb);
        TempData["Delete"] = "Item has Deleted Successfully";
        return RedirectToAction("Index");
    }
}
