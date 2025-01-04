namespace SocialMedia.API.Resources.UserResources
{
    public class ChangeEmailResource
    {
        public string UserName { get; set; }
        public string OldEmail { get; set; }
        public string NewEmail { get; set; }
    }
}
