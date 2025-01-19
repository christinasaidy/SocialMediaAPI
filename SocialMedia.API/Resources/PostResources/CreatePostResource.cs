namespace SocialMedia.API.Resources.PostResources
{
    public class CreatePostResource
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;

        //public List<string> ImagePaths { get; set; } = new List<string>();
    }
}
