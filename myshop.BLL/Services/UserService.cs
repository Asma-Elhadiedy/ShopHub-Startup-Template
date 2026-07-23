
namespace myshop.BLL.Services;

public class UserService(ILogger<UserService> _logger,
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    IFileService _fileService,
    RoleManager<IdentityRole> _roleManager,
    UserManager<ApplicationUser> _userManager) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync(string currentUserId)
    {
        //return await _unitOfWork.Repository<ApplicationUser>()
        //    .GetQueryable(null)
        //    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
        //    .ToListAsync();
 
        return await _userManager.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                RoleName = _userManager.GetRolesAsync(u).Result.FirstOrDefault() ?? "-",
                IsLocked = u.IsLocked,
                IsCurrentUser = u.Id == currentUserId
            }).ToListAsync();
 
        //return await _unitOfWork.Repository<ApplicationUser>()
        //    .GetQueryable(null)
        //    .Select(u => new UserDto
        //    {
        //        Id = u.Id,
        //        FullName = u.FullName,
        //        Email = u.Email,
        //        //RoleName = _userManager.GetRolesAsync(u).Result.FirstOrDefault() ?? "-",
        //        IsLocked = u.IsLocked,
        //        IsCurrentUser = u.Id == currentUserId
        //    }).ToListAsync();
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

    public async Task<UserVM> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return _mapper.Map<UserVM>(user);
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        var deletionResult = await _userManager.DeleteAsync(user);
        return deletionResult.Succeeded;
    }
}
