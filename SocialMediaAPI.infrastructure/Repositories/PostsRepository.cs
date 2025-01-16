using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using SocialMediaAPI.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaAPI.infrastructure.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private readonly ApplicationDbContext _context;

        public PostsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Posts?> GetPostByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<IEnumerable<Posts>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.Author)  
                .Include(p => p.Category) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Posts>> GetPostsByUserIdAsync(int userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.Author)  
                .Include(p => p.Category) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Posts>> GetPostsByCategoryIdAsync(int categoryId)
        {
            return await _context.Posts
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Author)  
                .Include(p => p.Category) 
                .ToListAsync();
        }

        public async Task<Posts> AddPostAsync(Posts post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return post; 
        }

        public async Task<Posts> UpdatePostAsync(Posts post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            return post; 
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Posts>> GetPostsSortedByUpvotesAsync(int count)
        {
            return await _context.Posts
                .Include(post => post.Author)  
                .Include(post => post.Category)
                .OrderByDescending(post => post.UpvotesCount)
                .Take(count)  // Limit the number of posts returned
                .ToListAsync();
        }
        public async Task<IEnumerable<Posts>> GetLatestPostsAsync(int count, int offset)
        {
            return await _context.Posts
                .OrderByDescending(p => p.CreatedAt)
                .Include(post => post.Author)
                .Include(post => post.Category)
                .Skip(offset)
                .Take(count)
                .ToListAsync();
        }


    }

}
