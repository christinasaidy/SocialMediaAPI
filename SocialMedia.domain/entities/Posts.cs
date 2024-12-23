using SocialMediaAPI.domain.entities;
using System.ComponentModel.DataAnnotations.Schema;

public class Posts
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public int UserId { get; set; } // This is the foreign key for the Author

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
    public Categories Category { get; set; } // Navigation property to Category
}
