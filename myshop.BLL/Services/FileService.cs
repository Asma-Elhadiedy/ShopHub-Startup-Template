

using Microsoft.Extensions.Hosting;

namespace myshop.BLL.Services;

public class FileService(IHostEnvironment _hostEnvironment) : IFileService
{

    public async Task<string> SaveFileAsync(IFormFile file, string folder)
    {
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

    public async Task<bool> DeleteFileAsync(string filePath)
    {
        var fullPath = Path.Combine(_hostEnvironment.ContentRootPath, ConstPath.WWWRootPath, filePath);
        if (!File.Exists(fullPath))
            return false;

        File.Delete(fullPath);
        return true;
    }
}