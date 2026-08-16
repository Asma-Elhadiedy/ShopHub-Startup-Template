
namespace myshop.BLL.ConfigurationOptions;

public class MailKitOptions
{
    public const string MailKit = "EmailSettings";

    [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535.")]
    public int Port { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Host { get; set; } = null!;

    [Required(AllowEmptyStrings = false)]
    public string SenderName { get; set; } = null!;

    [Required(AllowEmptyStrings = false)]
    public string SenderEmail { get; set; } = null!;

    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; } = null!;
}
