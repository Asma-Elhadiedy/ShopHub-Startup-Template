
namespace myshop.Web.Controllers.Admin;

[Authorize]
[Area("Admin")]
public class OrderController(IOrderService _orderService) : Controller
{
    public async Task<IActionResult> Index()
      => View();

    public async Task<IActionResult> GetData()
    {
        var data = await _orderService.GetAllOrdersAsync();
        return Json(new { data });
    }


}