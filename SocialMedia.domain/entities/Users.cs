﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.domain.entities
{
    public class Users
    {
        public int Id { get; set; } // Primary key


        [Column("Username")]
        public string UserName { get; set; } = string.Empty; // Not null

        public string Email { get; set; } = string.Empty; // Not null

        public string Password { get; set; } = string.Empty; // Not null

        public string Role { get; set; } = "user";

        public int ReputationPoints { get; set; } = 0; // Default zero

        public DateTime DateJoined { get; set; } = DateTime.UtcNow; // Default getdate() equivalent
        public string Bio { get; set; } = "Unedited Bio";
    }
}
