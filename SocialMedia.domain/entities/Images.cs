using System.ComponentModel.DataAnnotations.Schema;

public class Images
{
    public int Id { get; set; }

    public int PostId { get; set; }  // Foreign key to the Posts table

    public string ImagePath { get; set; } // Path or URL to the image file

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp when the image was added

    // Navigation property for the associated post
    [ForeignKey("PostId")]
    public Posts Post { get; set; }
}
