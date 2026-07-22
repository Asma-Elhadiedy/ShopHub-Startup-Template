

namespace myshop.BLL.Services;

public class UserService(IUnitOfWork _unitOfWork, IMapper _mapper, UserManager<ApplicationUser> _userManager) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        return await _unitOfWork.Repository<ApplicationUser>()
            .GetQueryable(null)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

}
