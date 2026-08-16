namespace myshop.BLL.DTOs.General;

public record SendEmailDto(
    string ToName,
    string ToEmail,
    string Subject,
    string Body,
    params string[] CCs);
