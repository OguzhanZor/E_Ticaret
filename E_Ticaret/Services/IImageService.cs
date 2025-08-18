using Microsoft.AspNetCore.Http;

namespace E_Ticaret.Services
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folderName);
        Task<bool> DeleteImageAsync(string imagePath);
        string GetImageUrl(string imagePath);
    }
}
