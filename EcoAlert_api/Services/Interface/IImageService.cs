namespace EcoAlert.Services.Interface
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile imageFile);
        Task<List<string>> UploadMultipleImageAsync(List<IFormFile> imageFiles);
        Task<bool> DeleteImageAsync(string imageUrl);
    }
}
