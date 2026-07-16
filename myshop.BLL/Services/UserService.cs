

namespace myshop.BLL.Services;

public class UserService(IUnitOfWork _unitOfWork, UserManager<ApplicationUser> _userManager) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        return _userManager.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name
            });
    }

}
