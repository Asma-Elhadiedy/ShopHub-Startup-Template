namespace myshop.Web.Controllers;

public class DashboardController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}
