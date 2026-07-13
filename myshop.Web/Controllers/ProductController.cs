
namespace myshop.Web.Controllers;

public class ProductController(IProductService _productService, IWebHostEnvironment _webHostEnvironment) : Controller
{
    public IActionResult Index()
        => View();


    public async Task<IActionResult> GetData()
    {
        var products = await _productService.GetAllProductsAsync();
        return Json(new { data = products });
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = await _productService.PrepareProductModelAsync(0);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductVM productVM, IFormFile file)
    {
        if (ModelState.IsValid)
        {
            string RootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string filename = Guid.NewGuid().ToString();
                var Upload = Path.Combine(RootPath, @"Images\Products");
                var ext = Path.GetExtension(file.FileName);

                using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                {
                    file.CopyTo(filestream);
                }
                productVM.Product.Img = @"Images\Products\" + filename + ext;
            }

            var isCreated = await _productService.CreateProductAsync(productVM);
            TempData["Create"] = "Item has Created Successfully";
            return RedirectToAction("Index");
        }
        return View(productVM.Product);
    }



    [HttpGet]
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
    public async Task<IActionResult> Edit(ProductVM productVM, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string RootPath = _webHostEnvironment.WebRootPath;

            if (file != null)
            {
                string filename = Guid.NewGuid().ToString();
                var Upload = Path.Combine(RootPath, @"Images\Products");
                var ext = Path.GetExtension(file.FileName);

                if (productVM.Product.Img != null)
                {
                    var oldimg = Path.Combine(RootPath, productVM.Product.Img.TrimStart('\\'));

                    if (System.IO.File.Exists(oldimg))
                    {
                        System.IO.File.Delete(oldimg);
                    }
                }

                using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                {
                    file.CopyTo(filestream);
                }

                productVM.Product.Img = @"Images\Products\" + filename + ext;
            }

            var isUpdated = await _productService.UpdateProductAsync(productVM);

            TempData["Update"] = "Data has Updated Successfully";
            return RedirectToAction("Index");
        }

        return View(productVM.Product);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var isDeleted = await _productService.DeleteProductAsync(id);
        if (isDeleted)
            return Json(new { success = true, message = "file has been Deleted" });
        return Json(new { success = false, message = "Error while Deleting" });
    }


}
