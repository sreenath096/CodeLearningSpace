using Azure.Storage.Blobs;
using LinkedInPost.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LinkedInPost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly BlobServiceClient _blobServiceClient;
        public BlobController(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

            [HttpGet("GetBlobs/{containerName}")]
            public async Task<ActionResult<List<BlobResponseDto>>> GetBlobs(string containerName)
            {
                if (!string.IsNullOrEmpty(containerName))
                {
                    var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                    var responseDtos = new List<BlobResponseDto>();
                    await foreach (var blobItem in blobContainerClient.GetBlobsAsync())
                    {
                        var responseDto = new BlobResponseDto()
                        {
                            Name = blobItem.Name,
                            VersionId = blobItem.VersionId,
                            Properties = new Properties()
                            {
                                CreatedOn = blobItem.Properties.CreatedOn,
                                LastModified = blobItem.Properties.LastModified,
                                ContentType = blobItem.Properties.ContentType                                                                            
                            }
                        };
                        responseDtos.Add(responseDto);
                    }
                    return Ok(responseDtos);
                }
                return BadRequest();
            }

        [HttpGet("GetBlobUrl/{containerName}/{blobName}")]
        public ActionResult<string> GetBlobUrl(string containerName, string blobName)
        {
            if (!string.IsNullOrEmpty(containerName) && !string.IsNullOrEmpty(blobName))
            {
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = blobContainerClient.GetBlobClient(blobName);
                return Ok(blobClient.Uri.AbsoluteUri);
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<bool> Delete([FromBody]BlobRequestDto blobRequestDto)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(blobRequestDto.ContainerName);
            var blobClient = blobContainerClient.GetBlobClient(blobRequestDto.BlobName);
            return await blobClient.DeleteIfExistsAsync();
        }                      
    }
}
