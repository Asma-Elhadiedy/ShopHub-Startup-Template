
namespace myshop.BLL.ViewModels.Accounts;

public class EditRoleVM
{
    public string UserId { get; set; } = null!;

    [DisplayName("Role")]
    [Required(ErrorMessage = ConstMessages.RequiredSelect)]
    public List<string> RoleIds { get; set; } = [];
    public IEnumerable<SelectListItem> Roles { get; set; } = [];
}