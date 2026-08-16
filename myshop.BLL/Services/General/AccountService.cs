
namespace myshop.BLL.Services.General;

public class AccountService(ILogger<AccountService> _logger,
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    IFileService _fileService,
    IEmailSenderService _emailSender,
    IHttpContextAccessor _httpContextAccessor,
    SignInManager<ApplicationUser> _signInManager,
    UserManager<ApplicationUser> _userManager) : IAccountService
{
    public async Task<(bool, string)> RegisterUserAsync(RegisterVM model, CancellationToken ct)
    {
        string? imagePath = null;

        try
        {
            var user = _mapper.Map<ApplicationUser>(model);
            var transactionResult = await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var creationResult = await _userManager.CreateAsync(user, model.Password);
                if (!creationResult.Succeeded)
                {
                    _logger.LogWarning("Failed to create user, identity errors: {errors}", string.Join(", ", creationResult.Errors.Select(e => e.Description)));
                    return (false, string.Join(", ", creationResult.Errors.Select(e => e.Description)));
                }

                imagePath = await _fileService.SaveFileAsync(model.Image, ConstPath.UserImagesPath);
                user.ImagePath = imagePath;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    _logger.LogWarning("Image update failed for {Email}.", model.Email);
                    return (false, "Failed to update user image.");
                }

                var roleAssignmentResult = await _userManager.AddToRoleAsync(user, ConstRoles.Customer);
                if (roleAssignmentResult.Succeeded)
                    return (true, "User registered successfully.");

                return (false, "Failed to assign user to role.");
            });

            if (!transactionResult.isSucess && imagePath is not null)
                _fileService.DeleteFile(imagePath);

            if (transactionResult.isSucess)
                await WelcomeNewUserAsync(model.FullName, model.Email, ct);

            return transactionResult;
        }
        catch (Exception ex)
        {
            if (imagePath is not null)
                _fileService.DeleteFile(imagePath);

            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<bool> SignInAsync(LoginVM model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                _logger.LogError("Sign in trial with non-existent email address.");
                return false;
            }

            if (user.IsLocked)
            {
                _logger.LogError("Sign in trial with a locked user account.");
                return false;
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                var session = _httpContextAccessor.HttpContext.Session;
                session.SetString("FullName", $"{user.FullName}");
                session.SetString("ImageUrl", user.ImagePath ?? ConstPath.DefaultUserImagePath);

                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }


    async Task<bool> WelcomeNewUserAsync(string name, string email, CancellationToken ct)
    {
        try
        {
            string subject = "Welcome to My Shop!";

            string htmlTemplate = await File.ReadAllTextAsync(Path.Combine(ConstPath.EmailTemplatesPath, ConstPath.WelcomeEmailTemplatePath), ct);

            var emailBody = htmlTemplate
                .Replace("{{UserName}}", name)
                .Replace("{{UserEmail}}", email)
                .Replace("{{StoreUrl}}", GetDomainPath())
                .Replace("{{CurrentYear}}", DateTime.UtcNow.Year.ToString());

            return await _emailSender.SendAsync(new SendEmailDto(name, email, subject, emailBody), ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }
    }

    string GetDomainPath()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null)
            return string.Empty;

        return $"{request.Scheme}://{request.Host}{request.PathBase}";
    }
}
