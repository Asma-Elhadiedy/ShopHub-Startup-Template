

namespace myshop.BLL.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<bool> CreateUserAsync(RegisterVM user);

}
