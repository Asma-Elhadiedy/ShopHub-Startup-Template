
namespace myshop.BLL.ViewModels.Accounts;

public class LoginVM
{
    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    [EmailAddress]
    public string Email { get; set; } = default!;


    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }

}
