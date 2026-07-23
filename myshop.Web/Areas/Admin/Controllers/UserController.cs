
namespace myshop.Web.Controllers.Admin;


[Area("Admin")]
[Authorize(Roles = ConstRoles.Admin)]
public class UserController(IUserService _userService) : Controller
{

    public async Task<IActionResult> Index()
        => View();

    public async Task<IActionResult> GetData()
    {
        var data = await _userService.GetAllUsersAsync(User.GetUserId());
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

    public async Task<IActionResult> Delete(string id)
    {
        if (id is null)
            return NotFound();
        
        if(id == User.GetUserId())
            return BadRequest();

        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var isSuccess = await _userService.DeleteUserAsync(id);
        if (isSuccess)
        {
            TempData["Delete"] = "User has been deleted successfully.";
            return RedirectToAction("Index");
        }
        else
        {
            TempData["Delete"] = "User could not be deleted.";
            return RedirectToAction("Index");
        }
    }
}
