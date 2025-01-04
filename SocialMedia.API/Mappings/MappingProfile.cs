using AutoMapper;
using SocialMediaAPI.domain.entities;
using SocialMedia.API.Resources.UserResources;
using Microsoft.Extensions.Hosting;
using SocialMedia.API.Resources.PostResources;

namespace SocialMedia.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map SignInResource to User entity (could be a simplified version for the login process)
            CreateMap<SignInResource, Users>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

            // Map SignUpResource to User entity (for registration)
            CreateMap<SignUpResource, Users>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

            CreateMap<CreatePostResource, Posts>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore()) // This will be set manually based on CategoryName
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}
