using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using SocialMediaAPI.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaAPI.infrastructure.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comments> GetCommentByIdAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.Post)
                    .ThenInclude(p => p.Author) 
                .Include(c => c.Post)
                    .ThenInclude(p => p.Category)
                .Include(c => c.User) 
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comments>> GetAllCommentsAsync()
        {
            return await _context.Comments
                .Include(c => c.Post)
                    .ThenInclude(p => p.Author) 
                .Include(c => c.Post)
                    .ThenInclude(p => p.Category) 
                .Include(c => c.User) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Comments>> GetCommentsByPostIdAsync(int postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.Post)
                    .ThenInclude(p => p.Author) 
                .Include(c => c.Post)
                    .ThenInclude(p => p.Category) 
                .Include(c => c.User) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Comments>> GetCommentsByUserIdAsync(int userId)
        {
            return await _context.Comments
                .Where(c => c.UserId == userId)
                .Include(c => c.Post)
                    .ThenInclude(p => p.Author) 
                .Include(c => c.Post)
                    .ThenInclude(p => p.Category) 
                .ToListAsync();
        }

        public async Task<Comments> AddCommentAsync(Comments comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comments> UpdateCommentAsync(Comments comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
