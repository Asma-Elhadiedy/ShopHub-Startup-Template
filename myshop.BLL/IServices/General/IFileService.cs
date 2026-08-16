namespace myshop.BLL.IServices.General;
public interface IFileService
{
    Task<string?> SaveFileAsync(IFormFile file, string folder);
    bool DeleteFile(string filePath);
}