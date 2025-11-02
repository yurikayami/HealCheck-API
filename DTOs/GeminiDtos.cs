namespace HealCheckAPI.DTOs
{
    public class GeminiNutritionResponse
    {
        public decimal Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Fat { get; set; }
        public decimal Carbohydrate { get; set; }
        public string Suggestion { get; set; } = string.Empty;
    }
}
