using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using SocialMediaAPI.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaAPI.infrastructure.Repositories
{
    public class VotesRepository : IVotesRepository
    {
        private readonly ApplicationDbContext _context;

        public VotesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Votes> GetVoteByIdAsync(int id)
        {
            return await _context.Votes
                .Include(v => v.Post)
                    .ThenInclude(p => p.Author) // Eager load the Author of the Post
                .Include(v => v.Post)
                    .ThenInclude(p => p.Category) // Eager load the Category of the Post
                .Include(v => v.User) // Eager load the User who cast the vote
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Votes>> GetAllVotesAsync()
        {
            return await _context.Votes
                .Include(v => v.Post)
                    .ThenInclude(p => p.Author) // Eager load the Author of the Post
                .Include(v => v.Post)
                    .ThenInclude(p => p.Category) // Eager load the Category of the Post
                .Include(v => v.User) // Eager load the User who cast the vote
                .ToListAsync();
        }

        public async Task<IEnumerable<Votes>> GetVotesByPostIdAsync(int postId)
        {
            return await _context.Votes
                .Where(v => v.PostId == postId)
                .Include(v => v.Post)
                    .ThenInclude(p => p.Author) // Eager load the Author of the Post
                .Include(v => v.Post)
                    .ThenInclude(p => p.Category) // Eager load the Category of the Post
                .Include(v => v.User) // Eager load the User who cast the vote
                .ToListAsync();
        }

        public async Task<IEnumerable<Votes>> GetVotesByUserIdAsync(int userId)
        {
            return await _context.Votes
                .Where(v => v.UserId == userId)
                .Include(v => v.Post)
                    .ThenInclude(p => p.Author) // Eager load the Author of the Post
                .Include(v => v.Post)
                    .ThenInclude(p => p.Category) // Eager load the Category of the Post
                .Include(v => v.User) // Eager load the User who cast the vote
                .ToListAsync();
        }

        public async Task<Votes> AddVoteAsync(Votes vote)
        {
            await _context.Votes.AddAsync(vote);
            await _context.SaveChangesAsync();
            return vote; // Return the added vote
        }

        public async Task<Votes> UpdateVoteAsync(Votes vote)
        {
            _context.Votes.Update(vote);
            await _context.SaveChangesAsync();
            return vote; // Return the updated vote
        }

        public async Task DeleteVoteAsync(int id)
        {
            var vote = await _context.Votes.FindAsync(id);
            if (vote != null)
            {
                _context.Votes.Remove(vote);
                await _context.SaveChangesAsync();
            }
        }
    }
}
