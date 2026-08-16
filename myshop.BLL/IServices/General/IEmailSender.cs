namespace myshop.BLL.IServices.General;

public interface IEmailSenderService
{
    Task<bool> SendAsync(SendEmailDto emailContentDto, CancellationToken ct = default);
}
