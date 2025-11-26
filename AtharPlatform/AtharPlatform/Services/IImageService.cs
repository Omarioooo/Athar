namespace AtharPlatform.Services
{
    public interface IImageService
    {
        string GetContentType(string extension);
        string GetContentType(byte[] imageData);
    }
}
