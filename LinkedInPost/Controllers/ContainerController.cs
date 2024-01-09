using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LinkedInPost.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LinkedInPost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerController : ControllerBase
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IMapper _mapper;

        public ContainerController(BlobServiceClient blobServiceClient
            , IMapper mapper)
        {
            _blobServiceClient = blobServiceClient;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<BlobContainerResponseDto>> GetAll()
        {
            var responseDtos = new List<BlobContainerResponseDto>();
            await foreach (var blobContainerItem in _blobServiceClient.GetBlobContainersAsync())
            {                
                responseDtos.Add(_mapper.Map<BlobContainerResponseDto>(blobContainerItem));
            }
            return responseDtos;
        }

        [HttpPost]
        public async Task<BlobContainerInfo> Create([FromBody] string containerName)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
        }

        [HttpDelete]
        public async Task<bool> Delete([FromBody] string containerName)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return await blobContainerClient.DeleteIfExistsAsync();
        }

        [HttpPost("UploadBlob")]
        public async Task<ActionResult<string>> UploadBlob([FromForm] BlobDto blobDto)
        {
            if (blobDto.UploadFile == null || blobDto.UploadFile.Length < 1)
            {
                ModelState.AddModelError("UploadFileError", "Please upload file");
                return BadRequest(ModelState);
            }

            var fileName = Path.GetFileNameWithoutExtension(blobDto.UploadFile.FileName);
            var fileExtension = Path.GetExtension(blobDto.UploadFile.FileName);
            var blobName = string.Concat(fileName, "_", Guid.NewGuid(), fileExtension);
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(blobDto.ContainerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            var blobHeaders = new BlobHttpHeaders()
            {
                ContentType = blobDto.UploadFile.ContentType
            };

            var blobMetadata = new Dictionary<string, string>()
            {
                {"uploadedBy", blobDto.UploadedBy }
            };
            if (!string.IsNullOrEmpty(blobDto.Notes))            
                blobMetadata.Add("notes", blobDto.Notes);                            
            
            var result = await blobClient.UploadAsync(blobDto.UploadFile.OpenReadStream(), blobHeaders, blobMetadata);

            if (result != null)
                return Ok(blobClient.Uri.AbsoluteUri);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
