namespace AtharPlatform.Services
{
    public class ImageService: IImageService
    {
        public string GetContentType(string extension)
        {
            if (string.IsNullOrEmpty(extension)) return "application/octet-stream";

            return extension.ToLower() switch
            {
                "png" => "image/png",
                "jpg" or "jpeg" => "image/jpeg",
                "gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }

        public string GetContentType(byte[] imageData)
        {
            if (imageData.Length < 4) return "application/octet-stream";

            if (imageData[0] == 0x89 && imageData[1] == 0x50 && imageData[2] == 0x4E && imageData[3] == 0x47)
                return "image/png";

            if (imageData[0] == 0xFF && imageData[1] == 0xD8)
                return "image/jpeg";

            if (imageData[0] == 0x47 && imageData[1] == 0x49 && imageData[2] == 0x46)
                return "image/gif";

            return "application/octet-stream";
        }
    }
}
