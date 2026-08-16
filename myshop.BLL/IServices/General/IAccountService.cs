namespace myshop.BLL.IServices.General;

public interface IAccountService
{
    Task<bool> SignInAsync(LoginVM model);
    Task<(bool, string)> RegisterUserAsync(RegisterVM model, CancellationToken ct);
    Task SignOutAsync();
}
