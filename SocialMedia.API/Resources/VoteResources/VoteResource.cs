namespace SocialMedia.API.Resources.VoteResources
{
    public class VoteResource
    {
        public int Id { get; set; }
        public string VoteType { get; set; }
        public string UserName { get; set; }
        public string PostTitle { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
    }
}
