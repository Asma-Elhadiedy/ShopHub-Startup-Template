
namespace myshop.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = ConstRoles.Admin)]
public class UserController(ILogger<UserController> _logger, IUserService _userService) : Controller
{

    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> GetData(CancellationToken ct)
    {
        var model = Request.Form.GetRequestForm();
        try
        {
            var usersDT = await _userService.GetAllUsersAsync(User.Id!, model, ct);
            return Json(usersDT);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get users for datatable with model: {model}", model);
            return BadRequest(new ResponseMessageDto
            {
                Title = ConstMessages.ErrorTitle,
                Message = ConstMessages.ErrorMessage
            });
        }
    }



    public IActionResult Create() => View(new RegisterVM());


    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RegisterVM model, CancellationToken ct)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(model);

            var (isCreated, message) = await _userService.CreateUserAsync(model, ct);
            if (isCreated)
                TempData["Success"] = ConstMessages.SuccessCreatedMessage;
            else
                TempData["Error"] = message;

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user with model: {model}", model);
            TempData["Error"] = ConstMessages.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }
    }



    public async Task<IActionResult> EditRole(string id, CancellationToken ct)
    {
        try
        {
            if (id is null)
                return BadRequest();

            var model = await _userService.PrepareEditRoleAsync(id, ct);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user roles for update: '{userId}'", id);
            TempData["Error"] = ConstMessages.ErrorFetchingDataMessage;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRole(EditRoleVM model)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(model);

            var (isSuccess, message) = await _userService.UpdateUserRolesAsync(model);
            if (isSuccess)
                TempData["Success"] = ConstMessages.SuccessUpdatedMessage;
            else
                TempData["Error"] = message;

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user roles with id: '{userId}' & model: {model}", User.Id, model);
            TempData["Error"] = ConstMessages.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }
    }



    public async Task<IActionResult> ChangeStatus(string id)
    {
        try
        {
            if (id is null)
                return BadRequest();

            if (id == User.Id)
            {
                TempData["Error"] = "Can't change your own account status.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return BadRequest();
            return View(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user data with id: '{userId}' to change status.", id);
            TempData["Error"] = ConstMessages.ErrorFetchingDataMessage;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatusConfirmed(string id, CancellationToken ct)
    {
        try
        {
            var isUpdated = await _userService.ChangeUserStatusAsync(id, ct);

            if (isUpdated)
                TempData["Success"] = ConstMessages.SuccessUpdatedMessage;
            else
                TempData["Error"] = ConstMessages.ErrorUpdatingMessage;

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to change user status with id: '{userId}' ", id);
            TempData["Error"] = ConstMessages.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }
    }




    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            if (id is null)
                return BadRequest();

            if (id == User.Id)
            {
                TempData["Error"] = "Can't delete your own account.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return BadRequest();
            return View(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user data with id: '{userId}', to delete.", id);
            TempData["Error"] = ConstMessages.ErrorFetchingDataMessage;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        try
        {
            var isDeleted = await _userService.DeleteUserAsync(id);
            if (isDeleted)
                TempData["Success"] = ConstMessages.SuccessDeletedMessage;
            else
                TempData["Error"] = ConstMessages.ErrorDeletingMessage;

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete user with id: '{userId}'.", id);
            TempData["Error"] = ConstMessages.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }
    }

}
