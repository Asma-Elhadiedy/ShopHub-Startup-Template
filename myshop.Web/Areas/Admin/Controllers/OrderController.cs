

namespace myshop.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(ConstCustomPolicies.AdminAndTechnicalSupportRole)]
public class OrderController(ILogger<OrderController> _logger, IAdminOrderService _orderService) : Controller
{
    public IActionResult Index()
      => View();


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