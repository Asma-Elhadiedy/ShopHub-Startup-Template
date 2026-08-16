

namespace myshop.DAL.Entities;

public class ApplicationRole : IdentityRole
{
    public ICollection<ApplicationUserRole>? UserRoles { get; set; } = [];
}