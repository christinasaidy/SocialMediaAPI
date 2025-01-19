using AutoMapper;
using SocialMediaAPI.domain.entities;
using SocialMedia.API.Resources.UserResources;
using Microsoft.Extensions.Hosting;
using SocialMedia.API.Resources.PostResources;
using SocialMedia.API.Resources.NotificationResources;
using SocialMedia.API.Resources.CommentResources;
using SocialMedia.API.Resources.CategoryResources;
using SocialMedia.API.Resources.VoteResources;

namespace SocialMedia.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SignInResource, Users>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

            CreateMap<SignUpResource, Users>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

            CreateMap<CreatePostResource, Posts>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.Ignore())   // Avoid direct mapping to Author
                .ForMember(dest => dest.Category, opt => opt.Ignore()) // Avoid direct mapping to Category
               .ForMember(dest => dest.Images, opt => opt.Ignore()); // Map ImagePaths to Image entitie;

            CreateMap<CreateNotificationResource, Notifications>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Recipient, opt => opt.Ignore());

            CreateMap<CreateCommentResource, Comments>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.PostId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<UpdateCommentResource, Comments>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.PostId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<CreateCategoryResource, Categories>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<UpdateCategoryResource, Categories>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<UpdatePostResource, Posts>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.Ignore())  // Avoid direct mapping to Author
                .ForMember(dest => dest.Category, opt => opt.Ignore()); // Avoid direct mapping to Category

            CreateMap<CreateVoteResource, Votes>()
                .ForMember(dest => dest.VoteType, opt => opt.MapFrom(src => src.VoteType))
                .ForMember(dest => dest.PostId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            // Add mapping from Post to its related Author and Category (to use in response)
            CreateMap<Posts, PostResource>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Upvotes, opt => opt.MapFrom(src => src.UpvotesCount))
                .ForMember(dest => dest.Downvotes, opt => opt.MapFrom(src => src.DownvotesCount))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));




            // If you want to return Author details in response (e.g. for GetPostById)
            CreateMap<Users, UserResource>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));

            // If you want to return Category details in response (e.g. for GetPostById)
            CreateMap<Categories, CategoryResource>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
