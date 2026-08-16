
using MimeKit;
using MimeKit.Text;

namespace myshop.BLL.Services.General;

public class MailKitSenderService(IOptions<MailKitOptions> _options, SmtpClient _client, ILogger<MailKitSenderService> _logger) : IEmailSenderService
{
    private readonly MailKitOptions _mailKitOptions = _options.Value;

    public async Task<bool> SendAsync(SendEmailDto emailContentDto, CancellationToken ct = default)
    {
        try
        {
            var message = new MimeMessage
            {
                Subject = emailContentDto.Subject,
            };

            message.From.Add(new MailboxAddress(_mailKitOptions.SenderName, _mailKitOptions.SenderEmail));
            message.To.Add(new MailboxAddress(emailContentDto.ToName, emailContentDto.ToEmail));
            if (emailContentDto.CCs.Length > 0)
                message.Cc.AddRange(emailContentDto.CCs.Select(cc => new MailboxAddress(cc, cc)));

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailContentDto.Body
            };

            await _client.ConnectAsync(_mailKitOptions.Host, _mailKitOptions.Port, false, ct);

            await _client.AuthenticateAsync(_mailKitOptions.SenderEmail, _mailKitOptions.Password, ct);
            var result = await _client.SendAsync(message, ct);
            await _client.DisconnectAsync(true, ct);

            _logger.LogError(result);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }
    }
}
