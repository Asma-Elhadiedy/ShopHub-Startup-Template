

namespace myshop.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(ConstCustomPolicies.AdminAndTechnicalSupportRole)]
public class OrderController(ILogger<OrderController> _logger, IAdminOrderService _orderService) : Controller
{
    public IActionResult Index()
      => View();


    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        if (id <= 0)
            return BadRequest();

        try
        {
            var model = await _orderService.GetOrderDetailsAsync(id, ct);
            if (model is null)
                return NotFound();

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve admin order details for order {OrderId}.", id);
            return BadRequest(new ResponseMessageDto
            {
                Title = ConstMessages.ErrorTitle,
                Message = ConstMessages.ErrorMessage
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> GetData(CancellationToken ct)
    {
        var model = Request.Form.GetRequestForm();
        try
        {
            var ordersDT = await _orderService.GetAllOrdersAsync(model, ct);
            return Json(ordersDT);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get orders to Datatable (POST). {model}", model);
            return BadRequest(new ResponseMessageDto
            {
                Title = ConstMessages.ErrorTitle,
                Message = ConstMessages.ErrorMessage
            });
        }
    }


}