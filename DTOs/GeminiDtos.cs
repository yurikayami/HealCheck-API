using System.Text.Json.Serialization;

namespace HealCheckAPI.DTOs
{
    public class GeminiNutritionResponse
    {
        [JsonPropertyName("foodName")]
        public string FoodName { get; set; } = string.Empty;

        [JsonPropertyName("calories")]
        public decimal Calories { get; set; }

        [JsonPropertyName("protein")]
        public decimal Protein { get; set; }

        [JsonPropertyName("fat")]
        public decimal Fat { get; set; }

        [JsonPropertyName("carbohydrate")]
        public decimal Carbohydrate { get; set; }

        [JsonPropertyName("suggestion")]
        public string Suggestion { get; set; } = string.Empty;
    }
}
