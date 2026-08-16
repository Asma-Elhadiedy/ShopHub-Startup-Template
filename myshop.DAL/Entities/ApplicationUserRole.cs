

namespace myshop.DAL.Entities;

public class ApplicationUserRole : IdentityUserRole<string>, IDomainModelMarker
{
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; }

    [ForeignKey(nameof(RoleId))]
    public ApplicationRole Role { get; set; }
}
