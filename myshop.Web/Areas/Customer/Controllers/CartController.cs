
namespace myshop.Web.Areas.Customer.Controllers;

[Area("Customer")]
public class CartController(ILogger<CartController> _logger, ICartService _cartService) : Controller
{
    public IActionResult Index() => View();
    
    public async Task<IActionResult> GetCart(CancellationToken ct)
    {
        try
        {
            if (HttpContext.Session.GetObject<CartVM>(ConstSession.CartContent) is CartVM cachedCartContent)
            {
                //if (User.Identity.IsAuthenticated && cachedCartContent.UserId is not null)
                return Json(cachedCartContent);
            }

            var cartId = HttpContext.Session.GetInt32(ConstSession.CartId) ?? 0;
            var cartContent = await RepopulateCartSession(cartId, User.Id, HttpContext.Session.Id, ct);
            return Json(cartContent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get cart data for user with id: '{userId}' ", User.Id);
            return BadRequest();
        }
    }


    public async Task<IActionResult> GetCartItemsCount(CancellationToken ct)
    {
        try
        {
            if (HttpContext.Session.GetObject<CartVM>(ConstSession.CartContent) is CartVM cachedCartContent)
            {
                if (User.Identity!.IsAuthenticated && cachedCartContent.UserId is null)
                    ResetCartSession();
                else
                    return Json(cachedCartContent.TotalItemsCount);
            }

            var cartId = HttpContext.Session.GetInt32(ConstSession.CartId) ?? 0;
            var cart = await RepopulateCartSession(cartId, User.Id, HttpContext.Session.Id, ct);
            return Json(cart.TotalItemsCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get cart items count for user with id: '{userId}'", User.Id);
            return BadRequest();
        }
    }



    [HttpPost]
    public async Task<IActionResult> AddCartItem(int id, CancellationToken ct)
    {
        AddCartItemVM model = new();
        try
        {
            if (id == 0)
                return BadRequest();

            model = new()
            {
                CartId = HttpContext.Session.GetObject<CartVM>(ConstSession.CartContent)?.Id ?? 0,
                ProductId = id,
                SessionId = HttpContext.Session.Id,
                UserId = User.Id
            };

            var iSuccess = await _cartService.AddUpdateCartAsync(model, ct);
            if (iSuccess)
            {
                ResetCartSession();
                return Ok(new ResponseMessageDto { Message = "The item is added to your cart.", });
            }
            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add cart item with model: {model}", model);
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity([FromBody] AddCartItemVM model, CancellationToken ct)
    {
        try
        {
            model.UserId = User.Id;
            model.SessionId = HttpContext.Session.Id;
            var isSuccess = await _cartService.AddUpdateCartItemQuantityAsync(model, ct);
            if (isSuccess)
            {
                ResetCartSession();
                return Ok();
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "failed to update cart item quantity with model: {model}", model);
            return BadRequest();
        }
    }



    [HttpPost]
    public async Task<IActionResult> RemoveCartItem(int id, CancellationToken ct)
    {
        try
        {
            if (id == 0)
                return BadRequest();

            var isSuccess = await _cartService.RemoveCartItemAsync(id, ct);
            if (isSuccess)
            {
                ResetCartSession();
                return Ok();
            }
            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete cart item with id: {id} for user with id: '{userId}'", id, User.Id);
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> ClearCart(int id, CancellationToken ct)
    {
        try
        {
            if (id == 0)
                return BadRequest();

            var isSuccess = await _cartService.ClearCartAsync(id, ct);
            if (isSuccess)
            {
                ResetCartSession(isClearCart: true);
                return Ok();
            }
            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete cart with id: {cartId}, for user with id: '{userId}'", id, User.Id);
            return BadRequest();
        }
    }



    public async Task<IActionResult> Checkout()
    {
        try
        {
            if (!User.Identity!.IsAuthenticated)
                return Ok(new { redirectUrl = Url.Action("Login", "Account", new { area = "", redirectUrl = "/Customer/Order/Checkout" }) });

            if (User.Identity.IsAuthenticated && !User.IsInRole(ConstRoles.Customer))
                return BadRequest(new ResponseMessageDto { Title = "Access Denied", Message = "Only customers can proceed to checkout." });

            return Ok(new { redirectUrl = Url.Action("checkout", "Order", new { area = "Customer" }) });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to checkout cart for user with id: '{userId}'", User.Id);
            return BadRequest();
        }
    }



    #region Helpers 
    public async Task<CartVM> RepopulateCartSession(int cartId, string? userId, string? sessionId, CancellationToken ct)
    {
        if (cartId == 0 && sessionId is null && userId is null)
            return new();

        CartVM cartContent = cartId > 0
           ? await _cartService.GetCartByIdAsync(cartId, userId, ct)
           : await _cartService.GetCartDataAsync(userId, sessionId, ct);

        HttpContext.Session.SetInt32(ConstSession.CartId, cartContent.Id);
        HttpContext.Session.SetObject(ConstSession.CartContent, cartContent);
        HttpContext.Session.SetInt32(ConstSession.TotalCartItemsCount, cartContent.TotalItemsCount);

        return cartContent;
    }
    public void ResetCartSession(bool isClearCart = false)
    {
        if (isClearCart)
            HttpContext.Session.Remove(ConstSession.CartId);

        HttpContext.Session.Remove(ConstSession.CartContent);
        HttpContext.Session.Remove(ConstSession.TotalCartItemsCount);
    }
    #endregion

}
