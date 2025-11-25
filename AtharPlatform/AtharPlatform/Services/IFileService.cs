namespace AtharPlatform.Services;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file, string folder);
    bool DeleteFile(string fileUrl);
    string GetFileUrl(string fileName, string folder);
}
