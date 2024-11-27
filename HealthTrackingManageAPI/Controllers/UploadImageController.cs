using HealthTrackingManageAPI.NewFolder.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadImageController : ControllerBase
    {
        private readonly CloudinaryService _cloudinaryService;
        private readonly ILogger<UploadImageController> _logger;

        public UploadImageController(CloudinaryService cloudinaryService,ILogger<UploadImageController> logger)
        {
            _cloudinaryService = cloudinaryService;
            _logger = logger;
        }

        
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadDto imageUpload)
        {
            try
            {
                // Upload ảnh
                var uploadResult = await _cloudinaryService.UploadImageAsync(
                    imageUpload.File,
                    "user_uploads"
                );

                return Ok(new
                {
                    message = "Upload thành công",
                    image = uploadResult
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi upload ảnh");
                return StatusCode(500, new
                {
                    message = "Có lỗi trong quá trình upload",
                    error = ex.Message
                });
            }
        }

        // Xóa ảnh
        [HttpDelete("delete/{publicId}")]
        public async Task<IActionResult> DeleteImage(string publicId)
        {
            try
            {
                var result = await _cloudinaryService.DeleteImageAsync(publicId);

                return result
                    ? Ok(new { message = "Xóa ảnh thành công" })
                    : BadRequest(new { message = "Không thể xóa ảnh" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xóa ảnh");
                return StatusCode(500, new
                {
                    message = "Có lỗi trong quá trình xóa",
                    error = ex.Message
                });
            }
        }
    }
}
