using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.domain.entities
{
    public class Comments
    {
        public int Id { get; set; } // Primary Key

        public int PostId { get; set; } // Foreign Key to Posts table
        public Posts Post { get; set; } // Navigation property to the Post

        public int UserId { get; set; } // Foreign Key to Users table
        public Users User { get; set; } // Navigation property to the User

        public string Content { get; set; } = string.Empty; // Comment content

        public DateTime CreatedAt { get; set; } // Comment creation date
    }
}
