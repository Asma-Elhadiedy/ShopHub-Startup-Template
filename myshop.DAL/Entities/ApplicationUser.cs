

namespace myshop.DAL.Entities;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
}
