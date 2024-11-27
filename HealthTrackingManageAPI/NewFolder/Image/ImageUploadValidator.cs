using FluentValidation;

namespace HealthTrackingManageAPI.NewFolder.Image
{
    public class ImageUploadValidator : AbstractValidator<ImageUploadDto>
    {
        public ImageUploadValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("Vui lòng chọn file")
                .Must(file => file.Length > 0).WithMessage("File không được để trống")
                .Must(file => file.Length <= 5 * 1024 * 1024) 
                    .WithMessage("Kích thước file tối đa là 5MB")
                .Must(file =>
                {
                    var allowedTypes = new[] {
                    "image/jpeg",
                    "image/png",
                    "image/gif",
                    "image/webp"
                    };
                    return allowedTypes.Contains(file.ContentType);
                }).WithMessage("Định dạng file không hợp lệ");
        }
    }
    
    public class ImageModel
    {
        public int Id { get; set; }
        public string PublicId { get; set; }
        public string Url { get; set; }
        public string OriginalFileName { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadDate { get; set; }
    }

   
    public class ImageUploadDto
    {
        public IFormFile File { get; set; }
        public string? Description { get; set; }
    }
}
