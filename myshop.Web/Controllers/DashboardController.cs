namespace myshop.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}
