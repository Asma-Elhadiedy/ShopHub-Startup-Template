
namespace myshop.Web.Controllers;

[Authorize]
public class UserController(IUserService _userService) : Controller
{

    public async Task<IActionResult> Index()
        => View();

    public async Task<IActionResult> GetData()
    {
        var data = await _userService.GetAllUsersAsync();
        return Json(new { data });
    }


    public IActionResult Create()
    {
        return View(new RegisterVM());
    }

    [HttpPost]
    public async Task<IActionResult> Create(RegisterVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var isSuccess = await _userService.CreateUserAsync(model);
        if (isSuccess)
            return RedirectToAction("Index");
        return View(model);
    }

}
