
namespace myshop.Web.Controllers;

public class AccountController(IAccountService _accountService) : Controller
{ 

    public async Task<IActionResult> Register()
        => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var isSuccess = await _accountService.RegisterUserAsync(model);
        if (isSuccess)
            return RedirectToAction(nameof(Login));
        return View(model);
    }

    public async Task<IActionResult> Login()
        => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var isSuccess = await _accountService.SignInAsync(model);
        if (isSuccess)
            return RedirectToAction("Index", "Category");
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _accountService.SignOutAsync();  
        return RedirectToAction("Login");
    }

}
