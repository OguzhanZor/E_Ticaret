using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace E_Ticaret.Services
{
    public class ImageService : IImageService
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly string _wwwrootPath;

        public ImageService(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _wwwrootPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot");
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile, string folderName)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Resim dosyası bulunamadı");

            // Dosya uzantısını kontrol et
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
                throw new ArgumentException("Sadece JPG, PNG, GIF ve WebP formatları desteklenir");

            // Dosya boyutunu kontrol et (5MB)
            if (imageFile.Length > 5 * 1024 * 1024)
                throw new ArgumentException("Resim dosyası 5MB'dan büyük olamaz");

            // Klasör yolunu oluştur
            var uploadFolder = Path.Combine(_wwwrootPath, "assets", "imgs", folderName);
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            // Benzersiz dosya adı oluştur
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadFolder, fileName);

            // Dosyayı kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Veritabanında saklanacak göreceli yolu döndür
            return $"~/assets/imgs/{folderName}/{fileName}";
        }

        public async Task<bool> DeleteImageAsync(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return false;

            try
            {
                // ~/ ile başlayan yolu wwwroot'a çevir
                var relativePath = imagePath.Replace("~/", "");
                var fullPath = Path.Combine(_wwwrootPath, relativePath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public string GetImageUrl(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return string.Empty;

            // ~/ ile başlayan yolu / ile değiştir
            return imagePath.Replace("~/", "/");
        }
    }
}
