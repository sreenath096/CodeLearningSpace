namespace LinkedInPost.Dtos
{
    public class BlobResponseDto
    {
        public string Name { get; set; }                
        public string VersionId { get; set; }       
        public Properties Properties { get; set; }
        public IDictionary<string, string> Metadata { get; set; }
    }
    public class Properties
    {
        public DateTimeOffset? LastModified { get; set; }                            
        public DateTimeOffset? CreatedOn { get; set; }
        public string ContentType { get; set; }
    }
}
