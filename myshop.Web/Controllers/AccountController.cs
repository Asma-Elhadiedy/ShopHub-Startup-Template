
namespace myshop.Web.Controllers;

public class AccountController(IAccountService _accountService) : Controller
{ 
    public async Task<IActionResult> Login()
        => View();

}
