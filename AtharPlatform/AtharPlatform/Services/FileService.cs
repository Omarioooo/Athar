namespace AtharPlatform.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FileService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
    {
        _env = env;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");

        // Generate unique filename
        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var uploadPath = Path.Combine(_env.WebRootPath, "uploads", folder);

        // Create directory if it doesn't exist
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var filePath = Path.Combine(uploadPath, fileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return URL
        return GetFileUrl(fileName, folder);
    }

    public bool DeleteFile(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
            return false;

        try
        {
            // Extract filename from URL
            var uri = new Uri(fileUrl);
            var relativePath = uri.AbsolutePath.TrimStart('/');
            var filePath = Path.Combine(_env.WebRootPath, relativePath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
        }
        catch
        {
            // Log error if needed
        }

        return false;
    }

    public string GetFileUrl(string fileName, string folder)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = $"{request?.Scheme}://{request?.Host}";
        return $"{baseUrl}/uploads/{folder}/{fileName}";
    }
}
