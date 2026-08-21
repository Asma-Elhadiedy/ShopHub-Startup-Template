
namespace myshop.BLL.ViewModels.Accounts;

public class LoginVM
{
    [EmailAddress]
    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    public string Email { get; set; } = default!;


    [DataType(DataType.Password)]
    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    public string Password { get; set; } = default!;

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
    public string? redirectUrl { get; set; }

}
