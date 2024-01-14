using System.ComponentModel.DataAnnotations;

namespace LinkedInPost.Dtos
{
    public class ContainerRequestDto
    {
        [Required]
        public string ContainerName { get; set; }
        [Required]
        public string UploadedBy { get; set; }
        public string? Notes { get; set; }
    }
}
