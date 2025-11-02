using HealCheckAPI.DTOs;

namespace HealCheckAPI.Services
{
    public interface IImageService
    {
        Task<ImageResponseDto?> UploadAndAnalyzeImageAsync(int userId, IFormFile imageFile);
        Task<ImageResponseDto?> GetImageAnalysisAsync(int imageId);
        Task<IEnumerable<ImageResponseDto>> GetAllImagesAsync(int? userId = null);
        Task<bool> DeleteImageAsync(int imageId);
    }
}
