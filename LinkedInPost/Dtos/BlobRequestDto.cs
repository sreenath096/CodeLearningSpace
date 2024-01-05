using System.ComponentModel.DataAnnotations;

namespace LinkedInPost.Dtos
{
    public class BlobRequestDto
    {
        [Required]
        public string ContainerName { get; set; }
        [Required]
        public string BlobName { get; set; }
    }
}
