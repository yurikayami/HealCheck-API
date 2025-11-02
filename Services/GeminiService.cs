using HealCheckAPI.DTOs;
using System.Text;
using System.Text.Json;

namespace HealCheckAPI.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GeminiService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<GeminiNutritionResponse?> AnalyzeImageAsync(string imagePath)
        {
            try
            {
                var apiKey = _configuration["GeminiAPI:ApiKey"];

                if (string.IsNullOrEmpty(apiKey))
                {
                    throw new InvalidOperationException("Gemini API key is not configured");
                }

                // Read the image file and convert to base64
                byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
                string base64Image = Convert.ToBase64String(imageBytes);
                string mimeType = GetMimeType(imagePath);

                // Prepare the request payload for Gemini API
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new object[]
                            {
                                new { text = "Analyze this food image and provide nutritional information. Return the response in JSON format with the following structure: {\"foodName\": \"<tên món ăn bằng tiếng Việt>\", \"calories\": <number>, \"protein\": <number in grams>, \"fat\": <number in grams>, \"carbohydrate\": <number in grams>, \"suggestion\": \"<health suggestion in Vietnamese>\"}. Only provide the JSON response without any additional text." },
                                new
                                {
                                    inline_data = new
                                    {
                                        mime_type = mimeType,
                                        data = base64Image
                                    }
                                }
                            }
                        }
                    }
                };

                var jsonContent = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Make request to Gemini API (using v1beta for newer models)
                var response = await _httpClient.PostAsync(
                    $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}",
                    content
                );

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Gemini API request failed: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var geminiResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);

                // Extract the text response
                var textResponse = geminiResponse
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                if (string.IsNullOrEmpty(textResponse))
                {
                    return null;
                }

                // Clean the response (remove markdown code blocks if present)
                textResponse = textResponse.Trim();
                if (textResponse.StartsWith("```json"))
                {
                    textResponse = textResponse.Substring(7);
                }
                if (textResponse.StartsWith("```"))
                {
                    textResponse = textResponse.Substring(3);
                }
                if (textResponse.EndsWith("```"))
                {
                    textResponse = textResponse.Substring(0, textResponse.Length - 3);
                }
                textResponse = textResponse.Trim();

                // Parse the nutritional data
                var nutritionData = JsonSerializer.Deserialize<GeminiNutritionResponse>(
                    textResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                return nutritionData;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use ILogger here)
                Console.WriteLine($"Error analyzing image: {ex.Message}");
                return null;
            }
        }

        private string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
    }
}
