namespace SocialMedia.API.Resources.UserResources
{
    public class SignUpResource
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
    }
}
