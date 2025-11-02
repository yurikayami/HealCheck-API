using HealCheckAPI.Data;
using HealCheckAPI.DTOs;
using HealCheckAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HealCheckAPI.Services
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext _context;
        private readonly IGeminiService _geminiService;
        private readonly IWebHostEnvironment _environment;

        public ImageService(
            ApplicationDbContext context,
            IGeminiService geminiService,
            IWebHostEnvironment environment)
        {
            _context = context;
            _geminiService = geminiService;
            _environment = environment;
        }

        public async Task<ImageResponseDto?> UploadAndAnalyzeImageAsync(int userId, IFormFile imageFile)
        {
            try
            {
                // Validate user exists
                var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
                if (!userExists)
                {
                    return null;
                }

                // Create uploads directory if it doesn't exist
                var uploadsFolder = Path.Combine(_environment.ContentRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Store relative path for URL construction
                var relativePath = $"/uploads/{fileName}";

                // Save the image file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Analyze the image using Gemini API
                var nutritionAnalysis = await _geminiService.AnalyzeImageAsync(filePath);

                if (nutritionAnalysis == null)
                {
                    // If analysis fails, still save the image but without nutrition data
                    var imageWithoutAnalysis = new Image
                    {
                        UserId = userId,
                        Path = relativePath, // Use relative path, not full path!
                        CreatedAt = DateTime.Now
                    };

                    _context.Images.Add(imageWithoutAnalysis);
                    await _context.SaveChangesAsync();

                    return new ImageResponseDto
                    {
                        Id = imageWithoutAnalysis.Id,
                        UserId = imageWithoutAnalysis.UserId,
                        Path = imageWithoutAnalysis.Path,
                        CreatedAt = imageWithoutAnalysis.CreatedAt
                    };
                }

                // Create image record
                var image = new Image
                {
                    UserId = userId,
                    Path = relativePath, // Use relative path, not full path!
                    Kcal = nutritionAnalysis.Calories,
                    FoodName = nutritionAnalysis.FoodName,
                    AiSuggestion = nutritionAnalysis.Suggestion,
                    CreatedAt = DateTime.Now
                };

                _context.Images.Add(image);
                await _context.SaveChangesAsync();

                // Get nutrients
                var nutrients = await _context.Nutrients.ToListAsync();

                // Create analysis records
                var analyses = new List<Analysis>
                {
                    new Analysis
                    {
                        ImageId = image.Id,
                        NutrientId = nutrients.First(n => n.Name == "Calories").Id,
                        Value = nutritionAnalysis.Calories,
                        Confidence = 0.95m,
                        CreatedAt = DateTime.Now
                    },
                    new Analysis
                    {
                        ImageId = image.Id,
                        NutrientId = nutrients.First(n => n.Name == "Protein").Id,
                        Value = nutritionAnalysis.Protein,
                        Confidence = 0.95m,
                        CreatedAt = DateTime.Now
                    },
                    new Analysis
                    {
                        ImageId = image.Id,
                        NutrientId = nutrients.First(n => n.Name == "Fat").Id,
                        Value = nutritionAnalysis.Fat,
                        Confidence = 0.95m,
                        CreatedAt = DateTime.Now
                    },
                    new Analysis
                    {
                        ImageId = image.Id,
                        NutrientId = nutrients.First(n => n.Name == "Carbohydrate").Id,
                        Value = nutritionAnalysis.Carbohydrate,
                        Confidence = 0.95m,
                        CreatedAt = DateTime.Now
                    }
                };

                _context.Analysis.AddRange(analyses);
                await _context.SaveChangesAsync();

                // Return response
                return await GetImageAnalysisAsync(image.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading and analyzing image: {ex.Message}");
                return null;
            }
        }

        public async Task<ImageResponseDto?> GetImageAnalysisAsync(int imageId)
        {
            var image = await _context.Images
                .Include(i => i.Analyses)
                    .ThenInclude(a => a.Nutrient)
                .FirstOrDefaultAsync(i => i.Id == imageId);

            if (image == null)
            {
                return null;
            }

            return new ImageResponseDto
            {
                Id = image.Id,
                UserId = image.UserId,
                Path = image.Path,
                ImagePath = $"/uploads/{Path.GetFileName(image.Path)}",
                Kcal = image.Kcal,
                Gam = image.Gam,
                FoodName = image.FoodName,
                AiSuggestion = image.AiSuggestion,
                CreatedAt = image.CreatedAt,
                NutrientAnalysis = image.Analyses.Select(a => new NutrientAnalysisDto
                {
                    NutrientName = a.Nutrient.Name,
                    Unit = a.Nutrient.Unit,
                    Value = a.Value,
                    Confidence = a.Confidence
                }).ToList()
            };
        }

        public async Task<IEnumerable<ImageResponseDto>> GetAllImagesAsync(int? userId = null)
        {
            var query = _context.Images
                .Include(i => i.Analyses)
                    .ThenInclude(a => a.Nutrient)
                .AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(i => i.UserId == userId.Value);
            }

            var images = await query.ToListAsync();

            return images.Select(image => new ImageResponseDto
            {
                Id = image.Id,
                UserId = image.UserId,
                Path = image.Path,
                ImagePath = $"/uploads/{Path.GetFileName(image.Path)}",
                Kcal = image.Kcal,
                Gam = image.Gam,
                FoodName = image.FoodName,
                AiSuggestion = image.AiSuggestion,
                CreatedAt = image.CreatedAt,
                NutrientAnalysis = image.Analyses.Select(a => new NutrientAnalysisDto
                {
                    NutrientName = a.Nutrient.Name,
                    Unit = a.Nutrient.Unit,
                    Value = a.Value,
                    Confidence = a.Confidence
                }).ToList()
            });
        }

        public async Task<bool> DeleteImageAsync(int imageId)
        {
            var image = await _context.Images
                .Include(i => i.Analyses)
                .FirstOrDefaultAsync(i => i.Id == imageId);

            if (image == null)
            {
                return false;
            }

            // Delete the physical file if it exists
            if (!string.IsNullOrEmpty(image.Path) && File.Exists(image.Path))
            {
                try
                {
                    File.Delete(image.Path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting file: {ex.Message}");
                }
            }

            // Delete analyses first (due to foreign key)
            if (image.Analyses != null && image.Analyses.Any())
            {
                _context.Analysis.RemoveRange(image.Analyses);
            }

            // Delete the image record
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
