
namespace myshop.Web.Controllers;

public class AccountController(IAccountService _accountService) : Controller
{ 

    public async Task<IActionResult> Register()
        => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM model)
    {
        var isSuccess = await _accountService.RegisterUserAsync(model);
        if (isSuccess)
            return RedirectToAction("Index", "Home");
        return View();
    }

    public async Task<IActionResult> Login()
        => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM model)
    {
        var isSuccess = await _accountService.SignInAsync(model);
        if (isSuccess)
            return RedirectToAction("Index", "Category");
        return View(isSuccess);
    }

    public async Task<IActionResult> Logout()
    {
        await _accountService.SignOutAsync();  
        return RedirectToAction("Login");
    }

}
