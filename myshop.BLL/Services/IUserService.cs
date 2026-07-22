

namespace myshop.BLL.Services;

public interface IUserService
{
    Task<bool> CreateUserAsync(RegisterVM user);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();

}
