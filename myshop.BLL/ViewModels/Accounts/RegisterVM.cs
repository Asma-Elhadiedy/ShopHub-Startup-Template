
namespace myshop.BLL.ViewModels.Accounts;

public class RegisterVM
{

    [EmailAddress]
    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    public string Email { get; set; } = string.Empty;

    public string UserName => Email;

    [Display(Name = "Full Name")]
    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    public string FullName { get; set; }


    [DataType(DataType.Password)]
    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    public string Password { get; set; } = string.Empty;


    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare(nameof(Password), ErrorMessage = ConstMessages.PasswordsDoNotMatch)]
    public string ConfirmPassword { get; set; } = default!;

    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    public IFormFile Image { get; set; }
    
    [ValidateNever]
    public string ImagePath { get; set; }

}
