using SocialMedia.API.Resources.CategoryResources;
using SocialMedia.API.Resources.UserResources;

namespace SocialMedia.API.Resources.PostResources
{
    public class PostResource
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }

        public CategoryResource Category { get; set; }
        public UserResource Author { get; set; }
    }
}
