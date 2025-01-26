using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using SocialMediaAPI.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            var posts = await _context.Posts.Where(p => p.UserId == userId).ToListAsync();
            _context.Posts.RemoveRange(posts);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
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

    }
}