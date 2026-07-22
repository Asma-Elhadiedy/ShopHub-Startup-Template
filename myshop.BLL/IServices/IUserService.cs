namespace myshop.BLL.IServices;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<bool> CreateUserAsync(RegisterVM user);

}
