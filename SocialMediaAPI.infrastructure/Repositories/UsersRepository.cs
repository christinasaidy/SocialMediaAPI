using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using SocialMediaAPI.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace SocialMediaAPI.infrastructure.Repositories
{
    public class UserRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Users> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users> AddUserAsync(Users user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Users> UpdateUserAsync(Users user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(int userId, string password)
        {
            // Fetch the user from the database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return false; // User not found
            }

            // Verify the provided password against the stored hashed password
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return false; // Incorrect password
            }

            // Delete all votes associated with the user's posts
            var posts = await _context.Posts.Where(p => p.UserId == userId).ToListAsync();
            foreach (var post in posts)
            {
                // Remove votes for the post
                var votes = await _context.Votes.Where(v => v.PostId == post.Id).ToListAsync();
                Console.WriteLine($"post: {post}");
                _context.Votes.RemoveRange(votes);

                // Remove comments for the post
                var comments = await _context.Comments.Where(c => c.PostId == post.Id).ToListAsync();
                _context.Comments.RemoveRange(comments);
            }

            // Remove all posts by the user
            _context.Posts.RemoveRange(posts);

            // Delete all comments directly made by the user
            var userComments = await _context.Comments.Where(c => c.UserId == userId).ToListAsync();
            _context.Comments.RemoveRange(userComments);

            // Delete all notifications associated with the user
            var notifications = await _context.Notifications.Where(n => n.SenderID == userId).ToListAsync();
            _context.Notifications.RemoveRange(notifications);

            // Finally, remove the user
            _context.Users.Remove(user);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return true; // User deleted successfully
        }



        public async Task<Users?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<string?> GetUsernameByIdAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user?.UserName;
        }

        public async Task<string?> GetEmailByIdAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user?.Email;
        }
        public async Task<IEnumerable<Posts>> GetPostsByUserIdAsync(int userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.Images)
                .ToListAsync();
        }


        public async Task<string?> GetBioByIdAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user?.Bio;
        }

        public async Task<bool> AddBioAsync(int userId, string bio)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            user.Bio = bio;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string?> GetProfilePictureByIdAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user?.ProfilePictureUrl;
        }

        public async Task<bool> AddProfilePictureAsync(int userId, string profilePictureUrl)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            user.ProfilePictureUrl = profilePictureUrl;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<int> GetPostCountByUserIdAsync(int userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .CountAsync();
        }

        public async Task<int> GetCommentCountByUserIdAsync(int userId)
        {
            return await _context.Comments
                .Where(c => c.UserId == userId)
                .CountAsync();
        }
        public async Task<int> GetEngagementCountByUserIdAsync(int userId)
        {
            return await _context.Votes
                .Where(v => v.UserId == userId)
                .CountAsync();
        }
        public async Task<bool> PatchUsernameAsync(int userId, string newUsername)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            // Check if the new username already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == newUsername);
            if (existingUser != null)
            {
                return false;
            }

            user.UserName = newUsername;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PatchEmailAsync(int userId, string newEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            // Check if the new user email already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == newEmail);
            if (existingUser != null)
            {
                return false;
            }

            user.Email = newEmail;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PatchPasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false; // User not found
            }

            // Verify current password
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.Password) || string.IsNullOrWhiteSpace(newPassword))
            {
                return false; // Current password is incorrect or new password is invalid
            }

            // Hash the new password
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

            // Update the user's password in the database
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true; // Password updated successfully
        }


    }
}