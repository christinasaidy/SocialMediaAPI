using SocialMediaAPI.Application.Interfaces;
using SocialMediaAPI.domain.entities;
using SocialMediaAPI.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaAPI.Infrastructure.Repositories
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
                .Include(p => p.Images) // Include the Images collection
                .FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task<IEnumerable<Posts>> GetPostsByUserIdAsync(int userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p=> p.Images)
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
            // Retrieve the post with the given id
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                // Delete votes associated with the post
                var votes = await _context.Votes.Where(v => v.PostId == id).ToListAsync();
                if (votes.Any())
                {
                    _context.Votes.RemoveRange(votes);
                }

                // Delete comments associated with the post
                var comments = await _context.Comments.Where(c => c.PostId == id).ToListAsync();
                if (comments.Any())
                {
                    _context.Comments.RemoveRange(comments);
                }

                // Delete the post itself
                _context.Posts.Remove(post);

                // Save all changes
                await _context.SaveChangesAsync();
            }
        }



        public async Task<IEnumerable<Posts>> GetPostsSortedByUpvotesAsync(int count)
        {
            return await _context.Posts
                .Include(post => post.Author)
                .Include(post => post.Category)
                .Include(post => post.Images)
                .OrderByDescending(post => post.UpvotesCount)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Posts>> GetLatestPostsAsync(int count, int offset)
        {
            return await _context.Posts
                .OrderByDescending(p => p.CreatedAt)
                .Include(post => post.Author)
                .Include(post => post.Category)
                .Include(post => post.Images)
                .Skip(offset)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetTotalPostsCountAsync()
        {
            return await _context.Posts.CountAsync();
        }

        // New methods for handling images
        public async Task AddImagesToPostAsync(int postId, IEnumerable<string> imagePaths)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null) return;

            // Create Image entities and store their paths in the database
            foreach (var path in imagePaths)
            {
                var image = new Images
                {
                    PostId = postId,
                    ImagePath = path, // Store the relative path
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Images.AddAsync(image);
            }

            await _context.SaveChangesAsync();
        }



        public async Task<IEnumerable<Images>> GetImagesByPostIdAsync(int postId)
        {
            return await _context.Images
                .Where(img => img.PostId == postId)
                .Include(img => img.Post)
                 .Include(img => img.Post.Author)
                 .Include(img => img.Post.Category)
                .ToListAsync();
        }
        public async Task<IEnumerable<Posts>> SearchPostsAsync(string query)
        {
            return await _context.Posts
                .Where(p => p.Title.Contains(query) ||
                            p.Description.Contains(query) ||
                            p.Tags.Contains(query) ||
                            p.Author.UserName.Contains(query) ||
                            _context.Categories.Any(c => c.Id == p.CategoryId && c.Name.Contains(query)))
                .OrderByDescending(p => p.UpvotesCount)
                .Include(post => post.Category) 
                .Include(post => post.Images)
                .Include(post => post.Author)
                .ToListAsync();
        }

    }
}
