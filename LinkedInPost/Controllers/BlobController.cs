using AutoMapper;
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
        private readonly IMapper _mapper;

        public BlobController(BlobServiceClient blobServiceClient
            , IMapper mapper)
        {
            _blobServiceClient = blobServiceClient;
            _mapper = mapper;
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
                    responseDtos.Add(_mapper.Map<BlobResponseDto>(blobItem));
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
