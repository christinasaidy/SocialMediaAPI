﻿namespace SocialMedia.API.Resources.UserResources
{
    public class UserResource
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; 
    }
}
