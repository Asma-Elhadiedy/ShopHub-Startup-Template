
namespace myshop.BLL.Services.Admin;

public class UserService(
    ILogger<UserService> _logger,
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    IFileService _fileService,
    RoleManager<ApplicationRole> _roleManager,
    UserManager<ApplicationUser> _userManager) : IUserService
{
    public async Task<PagingDTO<UserDto>> GetAllUsersAsync(string currentUserId, FormDto model, CancellationToken ct)
    {
        try
        {
            var usersQuery = _userManager.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    RoleIds = u.UserRoles.Select(ur => ur.RoleId).ToList(),
                    IsLocked = u.IsLocked,
                    IsCurrentUser = u.Id == currentUserId
                }).AsSplitQuery();

            if (!string.IsNullOrEmpty(model.Search?.Trim()))
                usersQuery = usersQuery.Where(u => u.FullName.Contains(model.Search));

            if (model.SortingCol is not null && model.SortingDir is not null)
                usersQuery = usersQuery.OrderBy($"{model.SortingCol} {model.SortingDir}");

            var recordsTotal = usersQuery.Count();
            var pagedUsers = await usersQuery
                  .Skip(model.Start)
                  .Take(model.PageSize)
                  .ToListAsync(ct);

            var distinctRoleIds = pagedUsers.SelectMany(u => u.RoleIds).Distinct().ToList();

            var roleNameById = await _roleManager.Roles
                .Where(r => distinctRoleIds.Contains(r.Id))
                .ToDictionaryAsync(r => r.Id, r => r.Name, ct);

            pagedUsers = [.. pagedUsers.Select(u =>
            {
                u.RoleNames = string.Join(", ", u.RoleIds.Select(id => roleNameById.GetValueOrDefault(id)));
                return u;
            })];

            return new()
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsTotal,
                Data = pagedUsers
            };

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<(bool isSuccess, string message)> CreateUserAsync(RegisterVM model, CancellationToken ct)
    {
        string? imagePath = null;

        try
        {
            var user = _mapper.Map<ApplicationUser>(model);
            var transactionResult = await _unitOfWork.ExecuteInTransactionAsync(async () =>
             {
                 var creationResult = await _userManager.CreateAsync(user, model.Password);
                 if (!creationResult.Succeeded)
                 {
                     _logger.LogWarning("Failed to create user, identity errors: {errors}", string.Join(", ", creationResult.Errors.Select(e => e.Description)));
                     return (false, string.Join(", ", creationResult.Errors.Select(e => e.Description)));
                 }

                 imagePath = await _fileService.SaveFileAsync(model.Image, ConstPath.UserImagesPath);
                 user.ImagePath = imagePath;

                 var updateResult = await _userManager.UpdateAsync(user);
                 if (!updateResult.Succeeded)
                 {
                     _logger.LogWarning("Image update failed for {Email}.", model.Email);
                     return (false, "Failed to update user image.");
                 }

                 var roleAssignmentResult = await _userManager.AddToRoleAsync(user, ConstRoles.Admin);
                 if (roleAssignmentResult.Succeeded)
                     return (true, "User created successfully.");

                 return (false, "Failed to assign user to role.");
             }, ct);

            if (!transactionResult.isSucess && imagePath is not null)
                _fileService.DeleteFile(imagePath);

            return transactionResult;
        }
        catch (Exception ex)
        {
            if (imagePath is not null)
                _fileService.DeleteFile(imagePath);

            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<UserVM> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            _logger.LogWarning("Failed to find user with id: {id}.", userId);
            return new();
        }
        return _mapper.Map<UserVM>(user);
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                _logger.LogWarning("Failed to find user with id: {id}.", userId);
                return false;
            }

            var deletionResult = await _userManager.DeleteAsync(user);
            if (deletionResult.Succeeded)
                _fileService.DeleteFile(user.ImagePath);

            return deletionResult.Succeeded;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<bool> ChangeUserStatusAsync(string userId, CancellationToken ct)
    {
        try
        {
            var user = await _unitOfWork.Repository<ApplicationUser>()
                .GetItemAsync(u => u.Id == userId, ct);

            if (user is null)
            {
                _logger.LogWarning("User with id {id} not found for status change.", userId);
                return false;
            }

            user.IsLocked = !user.IsLocked;
            if (await _unitOfWork.CompleteAsync(ct) > 0)
                return true;
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<EditRoleVM> PrepareEditRoleAsync(string userId, CancellationToken ct)
    {
        try
        {
            var roles = await GetAllRolesAsync(ct);

            return new EditRoleVM
            {
                RoleIds = [.. (await _unitOfWork.Repository<ApplicationUserRole>()
                    .GetAllSelectedAsync(
                        ur => ur.UserId == userId,
                        ur => ur.RoleId, ct))],
                UserId = userId,
                Roles = roles
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<(bool isSuccess, string message)> UpdateUserRolesAsync(EditRoleVM model)
    {
        try
        {
            if (!model.RoleIds.Any())
                return (false, "Select 1 role at least");

            var newRolesNames = await _roleManager.Roles
                    .Where(r => model.RoleIds.Contains(r.Id))
                    .Select(r => r.Name)
                    .ToListAsync() ?? Enumerable.Empty<string?>();
            if (!newRolesNames.Any())
            {
                _logger.LogWarning("No valid roles found for user id: {id}.", model.UserId);
                return (false, "No valid roles found.");
            }

            if (newRolesNames.Contains(ConstRoles.Customer) && model.RoleIds.Count() > 1)
            {
                _logger.LogWarning("Cannot assign 'Customer' role with other roles for user id: {id}.", model.UserId);
                return (false, "Cannot assign 'Customer' role with other roles.");
            }

            var transactionResult = await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<ApplicationUserRole>()
                    .BulkDeleteAsync(ur => ur.UserId == model.UserId);

                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user is null)
                {
                    _logger.LogWarning("Failed to find user to update his role with id: {id}.", model.UserId);
                    return (false, "Failed to find user.");
                }

                await _userManager.AddToRolesAsync(user, newRolesNames);
                return (true, "User roles updated successfully.");
            });

            return transactionResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private async Task<IList<SelectListItem>> GetAllRolesAsync(CancellationToken ct)
    {
        return await _roleManager.Roles.Select(r => new SelectListItem
        {
            Value = r.Id,
            Text = r.Name
        }).ToListAsync(ct);
    }

    private static IList<string> GetRoles(UserManager<ApplicationUser> _userManager, ApplicationUser user)
    {
        Console.WriteLine("Aloha");
        return _userManager.GetRolesAsync(user).Result;
        //return _userManager.GetRolesAsync(user).GetAwaiter().GetResult();
    }
}
