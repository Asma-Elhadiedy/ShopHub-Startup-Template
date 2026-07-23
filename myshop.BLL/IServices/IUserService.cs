namespace myshop.BLL.IServices;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync(string currentUserId);
    Task<bool> CreateUserAsync(RegisterVM user);
    Task<UserVM> GetUserByIdAsync(string id);
    Task<bool> DeleteUserAsync(string id);
}
