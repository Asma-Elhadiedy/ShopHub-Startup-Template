namespace myshop.BLL.DTOs.Admin;

public class UserDto
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string RoleNames { get; set; } = null!;
    public List<string> RoleIds { get; set; } = [];
    public bool IsLocked { get; set; }
    public bool IsCurrentUser { get; set; }
}
