

namespace myshop.BLL.Services;


public class UserService(ILogger<UserService> _logger,
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    IFileService _fileService,
    UserManager<ApplicationUser> _userManager) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()  
    {
        return await _unitOfWork.Repository<ApplicationUser>()
            .GetQueryable(null)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<bool> CreateUserAsync(RegisterVM model)
    {
        var isEmailExist = await _unitOfWork.Repository<ApplicationUser>()
            .IsExistAsync(u => u.Email == model.Email);
        if (isEmailExist)
        {
            _logger.LogError("Creation trial with an existent email address.");
            return false;
        }

        var user = _mapper.Map<ApplicationUser>(model);

        var identityResult = await _userManager.CreateAsync(user, model.Password);
        if (!identityResult.Succeeded)
            return false;

        user.ImagePath = await _fileService.SaveFileAsync(model.Image, ConstPath.UserImagesPath);
        await _userManager.UpdateAsync(user);

        var roleAssignmentResult = await _userManager.AddToRoleAsync(user, ConstRoles.Admin);
        if (roleAssignmentResult.Succeeded)
            return true;

        return false;
    }

}
