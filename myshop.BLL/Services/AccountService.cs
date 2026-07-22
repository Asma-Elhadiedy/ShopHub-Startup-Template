

namespace myshop.BLL.Services;

public class AccountService(ILogger<AccountService> _logger,
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    SignInManager<ApplicationUser> _signInManager,
    UserManager<ApplicationUser> _userManager) : IAccountService
{
    public async Task<bool> RegisterUserAsync(RegisterVM model)
    {

        var isEmailExist = await _unitOfWork.Repository<ApplicationUser>()
            .IsExistAsync(u => u.Email == model.Email);
        if (isEmailExist)
        {
            _logger.LogError("Register trial with an existent email address.");
            return false;
        }

        var user = _mapper.Map<ApplicationUser>(model);
        var identityResult = await _userManager.CreateAsync(user, model.Password);
        if (!identityResult.Succeeded)
            return false;

        var roleAssignmentResult = await _userManager.AddToRoleAsync(user, ConstRoles.Customer);
        if (roleAssignmentResult.Succeeded)
            return true;

        return false;
    }

    public async Task<bool> SignInAsync(LoginVM model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            _logger.LogError("Sign in trial with non-existent email address.");
            return false;
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
        if (result.Succeeded)
            return true;

        return false;
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
