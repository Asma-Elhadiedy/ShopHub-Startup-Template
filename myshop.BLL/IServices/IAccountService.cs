
namespace myshop.BLL.IServices;

public interface IAccountService
{
    Task<bool> SignInAsync(LoginVM model);
    Task<bool> RegisterUserAsync(RegisterVM model);
    Task SignOutAsync();
}
