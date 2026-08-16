
using myshop.BLL.DTOs.Admin;
using myshop.BLL.DTOs.General;

namespace myshop.BLL.IServices.Admin;

public interface IUserService
{
    Task<PagingDTO<UserDto>> GetAllUsersAsync(string currentUserId, FormDto model, CancellationToken ct);
    Task<(bool isSuccess, string message)> CreateUserAsync(RegisterVM user, CancellationToken ct);
    Task<UserVM> GetUserByIdAsync(string id);
    Task<bool> ChangeUserStatusAsync(string id, CancellationToken ct);
    Task<EditRoleVM> PrepareEditRoleAsync(string userId, CancellationToken ct);
    Task<(bool isSuccess, string message)> UpdateUserRolesAsync(EditRoleVM model);
    Task<bool> DeleteUserAsync(string id);
}
