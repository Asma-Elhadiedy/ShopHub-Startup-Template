
namespace myshop.Web.Areas.Customer.Controllers;

[Area("Customer")]
public class OrderController(ILogger<OrderController> _logger, IOrderService _orderService, StripeClient _client) : Controller
{
    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> GetData(CancellationToken ct)
    {
        var model = Request.Form.GetRequestForm();
        try
        {
            var ordersDT = await _orderService.GetAllOrdersAsync(model, User.Id!, ct);
            return Json(ordersDT);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get orders in GetData with model: {model}.", model);
            return BadRequest(new ResponseMessageDto
            {
                Title = ConstMessages.ErrorTitle,
                Message = ConstMessages.ErrorMessage
            });
        }
    }

    public async Task<IActionResult> Checkout(CancellationToken ct)
    {
        try
        {
            var model = await _orderService.PrepareDeliveryInfoModelAsync(User.Id!, ct);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to prepare delivery info for user id: '{customerId}' in Checkout.", User.Id);
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Checkout([FromBody] AddDeliveryInfoVM model, CancellationToken ct)
    {
        try
        {
            var cartId = HttpContext.Session.GetInt32(ConstSession.CartId) ?? 0;
            if (cartId == 0)
                return BadRequest();

            model.CartId = model.CartId == 0 ? cartId : model.CartId;
            model.ApplicationUserId = User.Id!;

            var (orderId, total) = await _orderService.CreateOrderFromCartAsync(model, User.Id!, User.EmailAddress!, ct);

            if (orderId == 0)
                return BadRequest();

            if (model.PaymentMethod != (int)ePaymentMethod.CashOnDelivery)
            {
                HttpContext.Session.SetString("orderTotal", total.ToString());
                TempData["OrderId"] = orderId;
                return Ok(new { redirectUrl = Url.Action("Payment") });
            }
            else
            {
                ResetCartSession(isClearCart: true);
                return Ok(new ResponseMessageDto { Title = "Success!", Message = "Your order is being prepared now!" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create order in Checkout (POST). {model}", model);
            return BadRequest();
        }
    }


    public IActionResult Payment() => View();

    [HttpPost]
    public async Task<IActionResult> CreatePaymentIntent(CancellationToken ct)
    {
        try
        {
            var amountString = HttpContext.Session.GetString("orderTotal");
            if (!long.TryParse(amountString, out long amount))
                return Json(new { clientSeret = "" });

            var paymentIntent = await _client.V1.PaymentIntents.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = amount * 100,
                Currency = "egp",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            }, cancellationToken: ct);

            return Json(new { clientSecret = paymentIntent.ClientSecret });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create payment intent in CreatePaymentIntent for user with id: '{userId}'.", User.Id);
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmedPayment(int id, CancellationToken ct)
    {
        try
        {
            if (id == 0)
                return BadRequest();

            var isOrderUpdated = await _orderService.UpdateOrderPaymentStatusAsync(id, ePaymentStatus.Paid, ct);
            if (!isOrderUpdated)
                return BadRequest();

            HttpContext.Session.Remove("orderTotal");
            ResetCartSession(isClearCart: true);
            return Ok(new ResponseMessageDto { Title = "Success!", Message = "Your order is being prepared now!" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to confirm payment in ConfirmedPayment for order id: {id}.", id);
            return BadRequest();
        }
    }



    public void ResetCartSession(bool isClearCart = false)
    {
        if (isClearCart)
            HttpContext.Session.Remove(ConstSession.CartId);

        HttpContext.Session.Remove(ConstSession.CartContent);
        HttpContext.Session.Remove(ConstSession.TotalCartItemsCount);
    }

}
