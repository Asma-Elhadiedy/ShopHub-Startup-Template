
namespace myshop.BLL.ViewModels.Accounts;

public class RegisterVM
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;
    [Required]
    [DataType(DataType.Password)]
    public string UserName => Email;

    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    public string FullName { get; set; }

    public string Password { get; set; } = default!;

    [Compare(nameof(Password), ErrorMessage = ConstMessages.PasswordsDoNotMatch)]
    public string ConfirmPassword { get; set; } = default!;

    public IFormFile Image { get; set; }
    
    [ValidateNever]
    public string ImagePath { get; set; }

}