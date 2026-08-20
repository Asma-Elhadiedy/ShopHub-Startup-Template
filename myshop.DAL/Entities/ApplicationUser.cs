

namespace myshop.DAL.Entities;


public class ApplicationUser : IdentityUser, IDomainModelMarker
{
    public string FullName { get; set; } = null!;
    public override string Email { get; set; } = null!;
    public string? Address { get; set; }
    public string? City { get; set; }
    public bool IsLocked { get; set; }
    public bool IsDeleted { get; set; }

    public string? ImagePath { get; set; }


    public ICollection<ShoppingCart>? Carts { get; set; } = [];
    public ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
    public ICollection<Review>? Reviews { get; set; } = [];
}
