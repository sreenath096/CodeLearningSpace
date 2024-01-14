namespace LinkedInPost.Dtos
{
    public class BlobContainerResponseDto
    {
        public string Name { get; set; }
        public string VersionId { get; set; }
        public string PublicAccess { get; set; }
        public IDictionary<string, string> Metadata { get; set; }
    }          
}
