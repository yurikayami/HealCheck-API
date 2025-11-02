using HealCheckAPI.DTOs;
using HealCheckAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealCheckAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        /// <summary>
        /// Upload an image for nutritional analysis
        /// </summary>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ImageResponseDto>> UploadImage([FromForm] ImageUploadRequestDto request)
        {
            if (request.Image == null || request.Image.Length == 0)
            {
                return BadRequest(new { message = "No image file provided" });
            }

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = Path.GetExtension(request.Image.FileName).ToLowerInvariant();

            if (!allowedExtensions.Any(ext => ext == fileExtension))
            {
                return BadRequest(new { message = "Invalid file type. Only image files are allowed." });
            }            // Validate file size (e.g., max 10MB)
            if (request.Image.Length > 10 * 1024 * 1024)
            {
                return BadRequest(new { message = "File size exceeds 10MB limit" });
            }

            var result = await _imageService.UploadAndAnalyzeImageAsync(request.UserId, request.Image); if (result == null)
            {
                return BadRequest(new { message = "Failed to upload and analyze image. Please check if the user exists." });
            }

            return Ok(result);
        }

        /// <summary>
        /// Get nutritional analysis result for a specific image
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageResponseDto>> GetImageAnalysis(int id)
        {
            var result = await _imageService.GetImageAnalysisAsync(id);

            if (result == null)
            {
                return NotFound(new { message = "Image not found" });
            }

            return Ok(result);
        }

        /// <summary>
        /// Get all images (optionally filter by userId)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageResponseDto>>> GetAllImages([FromQuery] int? userId = null)
        {
            var images = await _imageService.GetAllImagesAsync(userId);
            return Ok(images);
        }

        /// <summary>
        /// Delete an image
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteImage(int id)
        {
            var result = await _imageService.DeleteImageAsync(id);

            if (!result)
            {
                return NotFound(new { message = "Image not found" });
            }

            return Ok(new { message = "Image deleted successfully" });
        }
    }
}
