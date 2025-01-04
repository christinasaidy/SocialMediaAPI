namespace SocialMedia.API.Resources
{
    public class ChangePasswordResource
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
