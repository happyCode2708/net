namespace MyApi.Application.Common.Utils
{
    public static class FileUtils
    {
        public static string EncodeImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }

    }
}