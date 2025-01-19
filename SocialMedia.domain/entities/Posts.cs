using SocialMediaAPI.domain.entities;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

public class Posts
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public int UserId { get; set; } 

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Tags { get; set; } = string.Empty;

    public int UpvotesCount { get; set; } = 0;

    public int DownvotesCount { get; set; } = 0;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [ForeignKey("UserId")]
    public Users Author { get; set; }

    [ForeignKey("CategoryId")]
    public Categories Category { get; set; }

    public ICollection<Images> Images { get; set; } = new List<Images>(); // This represents the images associated with the post
}

