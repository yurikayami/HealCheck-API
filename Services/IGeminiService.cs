using HealCheckAPI.DTOs;

namespace HealCheckAPI.Services
{
    public interface IGeminiService
    {
        Task<GeminiNutritionResponse?> AnalyzeImageAsync(string imagePath);
    }
}
