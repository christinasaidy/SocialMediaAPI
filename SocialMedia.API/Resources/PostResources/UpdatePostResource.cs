namespace SocialMedia.API.Resources.PostResources
{
    public class UpdatePostResource
    {
        public string Title { get; set; } = string.Empty;  
        public string Description { get; set; } = string.Empty;  
        public string Tags { get; set; } = string.Empty; 
        public string CategoryName { get; set; } = string.Empty; 
    }
}
