using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using SocialMediaAPI.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
            // Fetch the user without loading posts since the foreign key is in the Posts entity
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false; // User not found
            }

            // Delete associated posts by userId
            var posts = await _context.Posts
                .Where(p => p.UserId == userId)
                .ToListAsync();

            _context.Posts.RemoveRange(posts);

            // Delete the user
            _context.Users.Remove(user);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Users?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        // New method to get the username by userId
        public async Task<string?> GetUsernameByIdAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user?.UserName; // Return the username, or null if the user is not found
        }
        public async Task<IEnumerable<Posts>> GetPostsByUserIdAsync(int userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId) // Filter posts by userId
                .ToListAsync(); // Convert to a list
        }
    }
}
