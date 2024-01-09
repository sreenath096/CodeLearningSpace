using AutoMapper;
using Azure.Storage.Blobs.Models;
using LinkedInPost.Dtos;

namespace LinkedInPost.Common
{
    public class AutoMapperMappingProfile : Profile
    {
        public AutoMapperMappingProfile()
        {
            // CreateMap<SourceType, DestinationType>();
            CreateMap<BlobContainerItem, BlobContainerResponseDto>()
                .ForMember(dest => dest.PublicAccess, 
                src => src.MapFrom(x => x.Properties.PublicAccess != null 
                ? x.Properties.PublicAccess.ToString() : string.Empty));
            CreateMap<BlobItem, BlobResponseDto>();
            CreateMap<BlobItemProperties, Properties>();            
        }
    }
}
