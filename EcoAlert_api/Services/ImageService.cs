using EcoAlert.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace EcoAlert.Services
{
    public class ImageService : IImageService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ImageService> _logger;
        public ImageService(
            IConfiguration configuration,
            IWebHostEnvironment environment,
            ILogger<ImageService> logger)
        {
            _configuration = configuration;
            _environment = environment;
            _logger = logger;
        }
        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl))
                    return false;
                // Extract filename from URL
                var fileName = Path.GetFileName(imageUrl);
                var filePath = Path.Combine(_environment.WebRootPath, "uploads", "issues", fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation($"Image deleted: {fileName}");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image");
                return false;
            }
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                    throw new ArgumentException("No image file provided");
                if (imageFile.Length > 5 * 1024 * 1024) // 5MB limit
                    throw new ArgumentException("Image size exceeds 5MB limit");
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                    throw new ArgumentException("Invalid image format ");
                // Create unique filename
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "issues");
                // Ensure directory exists
                Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);

                // Compress and resize image
                await using (var imageStream = imageFile.OpenReadStream())
                using (var image = await Image.LoadAsync(imageStream))
                {
                    // Resize if too large
                    if (image.Width > 1920 || image.Height > 1080)
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(1920, 1080),
                            Mode = ResizeMode.Max
                        }));
                    }

                    // Save with compression
                    await image.SaveAsync(filePath);
                }
                _logger.LogInformation($"Image uploaded: {fileName}");

                // Return relative URL
                return $"/uploads/issues/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image");
                throw;
            }
        }

        public async Task<List<string>> UploadMultipleImageAsync(List<IFormFile> imageFiles)
        {
           var uploadedUrls=new List<string>();
            foreach (var imageFile in imageFiles)
            {
                try
                {
                    var url = await UploadImageAsync(imageFile);
                    uploadedUrls.Add(url);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to upload image: {imageFile.FileName}");
                    
                }
            }

            return uploadedUrls;
        }

        // Helper method to create thumbnail
        public async Task<string> CreateThumbnailAsync(IFormFile imageFile, int width = 300, int height = 300)
        {
            try
            {
                var originalUrl = await UploadImageAsync(imageFile);
                var fileName = Path.GetFileName(originalUrl);
                var thumbFileName = $"thumb_{fileName}";

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "issues");
                var originalPath = Path.Combine(uploadsFolder, fileName);
                var thumbPath = Path.Combine(uploadsFolder, thumbFileName);

                using (var image = await Image.LoadAsync(originalPath))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(width, height),
                        Mode = ResizeMode.Crop
                    }));

                    await image.SaveAsync(thumbPath);
                }

                return $"/uploads/issues/{thumbFileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating thumbnail");
                throw;
            }
        }
    }
}
