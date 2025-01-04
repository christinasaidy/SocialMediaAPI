namespace SocialMedia.API.Resources.CommentResources
{
    public class CreateCommentResource
    {
        public int PostId { get; set; }  // The ID of the post being commented on
        public string Content { get; set; }
    }
}
