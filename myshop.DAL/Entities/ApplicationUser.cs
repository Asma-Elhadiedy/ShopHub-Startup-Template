

namespace myshop.DAL.Entities;


public class ApplicationUser : IdentityUser, IDomainModelMarker 
{
    public string FullName { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public bool IsLocked { get; set; } 

    public string? ImagePath { get; set; }

}
