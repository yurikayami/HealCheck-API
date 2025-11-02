namespace HealCheckAPI.DTOs
{
    public class ImageUploadDto
    {
        public IFormFile Image { get; set; } = null!;
    }

    public class ImageUploadRequestDto
    {
        public int UserId { get; set; }
        public IFormFile Image { get; set; } = null!;
    }

    public class ImageResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Path { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public decimal? Kcal { get; set; }
        public decimal? Gam { get; set; }
        public string? AiSuggestion { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<NutrientAnalysisDto> NutrientAnalysis { get; set; } = new();
    }

    public class NutrientAnalysisDto
    {
        public string NutrientName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal? Confidence { get; set; }
    }
}
