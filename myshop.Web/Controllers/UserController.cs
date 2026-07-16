
namespace myshop.Web.Controllers;

public class UserController(IUserService _userService) : Controller
{

    public async Task<IActionResult> Index()
        => View();

    public async Task<IActionResult> GetData()
    {
        var data = await _userService.GetAllUsersAsync();
        return Json(new { data });
    }

}
