using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Principal;

namespace HealthTrackingManageAPI.NewFolder.Image
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;

        public CloudinaryService(
            IConfiguration configuration,
            ILogger<CloudinaryService> logger)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
            _logger = logger;
        }

        public async Task<ImageModel> UploadImageAsync(
            IFormFile file,
            string folder = "default")
        {
            try
            {
                
                var fileName = Guid.NewGuid().ToString();

                
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName, file.OpenReadStream()),
                    Folder = folder,

                    
                    Transformation = new Transformation()
                        .Width(1200)  
                        .Height(1200) 
                        .Crop("limit") 
                };

               
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

               
                return new ImageModel
                {
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.Url.ToString(),
                    OriginalFileName = file.FileName,
                    FileSize = file.Length,
                    UploadDate = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Upload image failed");
                throw;
            }
        }

        
        public async Task<bool> DeleteImageAsync(string publicId)
        {
            try
            {
                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);
                return result.Result == "ok";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete image failed");
                return false;
            }
        }
    }
}
