

namespace myshop.BLL.Services.General;

public class LocalFileService(IHostEnvironment _hostEnvironment, ILogger<LocalFileService> _logger) : IFileService
{

    public async Task<string?> SaveFileAsync(IFormFile file, string folder)
    {
        try
        {
            if (!IsUploadedImageValid(file))
                return null;

            var uploadPath = Path.Combine(_hostEnvironment.ContentRootPath, ConstPath.WWWRootPath, folder);
            Directory.CreateDirectory(uploadPath);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(folder, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return null;
        }
    }

    public bool DeleteFile(string filePath)
    {
        try
        {
            var fullPath = Path.Combine(_hostEnvironment.ContentRootPath, ConstPath.WWWRootPath, filePath);
            if (!File.Exists(fullPath))
            {
                _logger.LogError("Failed to find file for deletion in path: {path}", filePath);
                return false;
            }
            File.Delete(fullPath);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete file in path: {path}", filePath);
            return false;
        }
    }

    bool IsUploadedImageValid(IFormFile file)
    {
        if (file.Length > 2 * 1024 * 1024) //2 MB
            return false;

        string[] validContentType = ["image/jpeg", "image/png", "image/webp"];
        if (!validContentType.Contains(file.ContentType))
            return false;

        return true;
    }
}