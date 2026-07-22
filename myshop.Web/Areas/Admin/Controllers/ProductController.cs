

namespace myshop.Web.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = ConstRoles.Admin)]
public class ProductController(IProductService _productService, IWebHostEnvironment _webHostEnvironment) : Controller
{
    public IActionResult Index()
        => View();


    public async Task<IActionResult> GetData()
    {
        var products = await _productService.GetAllProductsAsync();
        return Json(new { data = products });
    }



    public async Task<IActionResult> Create()
    {
        var model = await _productService.PrepareProductModelAsync(0);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductVM model)
    {
        if (ModelState.IsValid)
        {
            var isCreated = await _productService.CreateProductAsync(model);

            TempData["Create"] = isCreated
                ? "Item has Created Successfully"
                : "Item has not Created Successfully";

            return RedirectToAction("Index");
        }
        return View(model);
    }




    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var model = await _productService.PrepareProductModelAsync(id!.Value);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductVM model)
    {
        ModelState.Remove(nameof(model.File));
        if (ModelState.IsValid)
        {
            var isUpdated = await _productService.UpdateProductAsync(model);

            TempData["Update"] = isUpdated
                ? "Data has Updated Successfully"
                : "Data has not Updated Successfully";

            return RedirectToAction("Index");
        }

        return View(model);
    }



    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var model = await _productService.PrepareProductModelAsync(id!.Value);
        return View(model);
    }

    
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var isDeleted = await _productService.DeleteProductAsync(id);
        TempData["Delete"] = isDeleted 
            ? "file has been Deleted" 
            : "Error while Deleting";

        return RedirectToAction("Index");

        //return Json(new { success = true, message = "file has been Deleted" });
        //return Json(new { success = false, message = "Error while Deleting" });

    }


}
