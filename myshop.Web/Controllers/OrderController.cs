
namespace myshop.Web.Controllers;

[Authorize]
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