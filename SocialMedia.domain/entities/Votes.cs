using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.domain.entities
{
    public class Votes
    {
        public int Id { get; set; } 

        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public Posts Post { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public Users User { get; set; } 

        public string VoteType { get; set; } = string.Empty; 
    }
}
