using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
        public async Task<ActionResult<List<BlobItem>>> GetBlobs(string containerName)
        {
            if (!string.IsNullOrEmpty(containerName))
            {
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobItems = new List<BlobItem>();
                await foreach (var blobItem in blobContainerClient.GetBlobsAsync())
                {
                    blobItems.Add(blobItem);
                }
                return Ok(blobItems);
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
