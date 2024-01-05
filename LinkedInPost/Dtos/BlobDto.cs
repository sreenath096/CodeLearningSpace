using LinkedInPost.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace LinkedInPost.Dtos
{
    public class BlobDto
    {
        [Required]
        public string ContainerName { get; set; }
        [VaildExtension(".png")]
        public IFormFile? UploadFile { get; set; }
        public string UploadedBy { get; set; }
        public string UploadedDate { get; set; }
    }
}
