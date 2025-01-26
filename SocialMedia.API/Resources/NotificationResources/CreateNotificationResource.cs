namespace SocialMedia.API.Resources.NotificationResources
{
    public class CreateNotificationResource
    {
        public string NotificationType { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public int PostId { get; set; }
        public int ReceiverId { get; set; }
    }
}
