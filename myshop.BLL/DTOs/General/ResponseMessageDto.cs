namespace myshop.BLL.DTOs.General;

public class ResponseMessageDto
{
    public string? Title { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class ResponseMessageDto<T> : ResponseMessageDto
{
    public T? Data { get; set; }
}