
namespace myshop.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = ConstRoles.Admin)]
public class SettingController(ILogger<SettingController> _logger, IAdminSettingsService _settingsService) : Controller
{
    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> RestoreDeletedProducts()
    {
        try
        {
            var restoredCount = await _settingsService.RestoreDeletedProductsAsync(User.Id!);
            if (restoredCount > 0)
                return Ok(new ResponseMessageDto { Title = ConstMessages.SuccessTitle, Message = $"{restoredCount} products restored successfully." });
            return Ok(new ResponseMessageDto { Title = ConstMessages.SuccessTitle, Message = $"No deleted products to restore." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to restore soft deleted products by user with id: {userId}", User.Id);
            return BadRequest(new ResponseMessageDto { Title = ConstMessages.ErrorTitle, Message = ConstMessages.ErrorMessage });
        }
    }
}
