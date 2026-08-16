
namespace myshop.Web.Controllers;

public class AccountController(ILogger<AccountController> _logger, IAccountService _accountService) : Controller
{
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM model, CancellationToken ct)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(model);

            var (isSuccess, message) = await _accountService.RegisterUserAsync(model, ct);
            if (isSuccess)
                return RedirectToAction(nameof(Login));

            ModelState.AddModelError(string.Empty, message);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to register user with data; {model}", model);
            ModelState.AddModelError(nameof(model.Password), "Error happened while registering, contact technical support");
            return View(model);
        }
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM model)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(model);

            var isSuccess = await _accountService.SignInAsync(model);
            if (isSuccess)
            {
                //User has added products pre-login
                var cartId = HttpContext.Session.GetInt32(ConstSession.CartId) ?? 0;
                if (cartId > 0)
                    TempData["BeforeLoginCartId"] = cartId;

                return RedirectToRoleBased();
            }
            ModelState.AddModelError(nameof(model.Password), "Incorrect username or password");
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to login.");
            ModelState.AddModelError(nameof(model.Password), "Error happened while signing in, contact technical support");
            return View(model);
        }
    }

    private RedirectToActionResult RedirectToRoleBased()
    {
        if (User.IsInRole(ConstRoles.Admin))
            return RedirectToAction("Category", "Admin");

        return RedirectToAction("Home", "Customer");
    }

    public async Task<IActionResult> Logout()
    {
        await _accountService.SignOutAsync();
        return RedirectToAction("Home", "Customer");
    }

}
