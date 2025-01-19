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
                    .ThenInclude(p => p.Author)
                .Include(v => v.Post)
                    .ThenInclude(p => p.Category)
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Votes>> GetVotesByPostIdAsync(int postId)
        {
            return await _context.Votes
                .Where(v => v.PostId == postId)
                .Include(v => v.Post)
                    .ThenInclude(p => p.Author)
                .Include(v => v.Post)
                    .ThenInclude(p => p.Category)
                .Include(v => v.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Votes>> GetVotesByUserIdAsync(int userId)
        {
            return await _context.Votes
                .Where(v => v.UserId == userId)
                .Include(v => v.Post)
                    .ThenInclude(p => p.Author)
                .Include(v => v.Post)
                    .ThenInclude(p => p.Category)
                .Include(v => v.User)
                .ToListAsync();
        }

        public async Task<Votes> AddVoteAsync(Votes vote)
        {
            await _context.Votes.AddAsync(vote);
            await _context.SaveChangesAsync();
            return vote;
        }

        public async Task<Votes> UpdateVoteAsync(Votes vote)
        {
            _context.Votes.Update(vote);
            await _context.SaveChangesAsync();
            return vote;
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

        public async Task<Posts> GetPostByIdAsync(int postId)
        {
            return await _context.Posts
                .FirstOrDefaultAsync(p => p.Id == postId);
        }

        public async Task<Votes> GetVoteByUserAndPostAsync(int userId, int postId)
        {
            return await _context.Votes
                .FirstOrDefaultAsync(v => v.UserId == userId && v.PostId == postId);
        }

        // New method to get the current vote status (upvoted/downvoted)
        public async Task<string> GetVoteStatusAsync(int userId, int postId)
        {
            var vote = await _context.Votes
                .FirstOrDefaultAsync(v => v.UserId == userId && v.PostId == postId);

            return vote?.VoteType; // Could return "Upvote", "Downvote", or null if no vote exists
        }

        // New method to handle adding or updating a vote
        public async Task<bool> HandleVoteAsync(int userId, int postId, string voteType)
        {
            var existingVote = await _context.Votes
                .FirstOrDefaultAsync(v => v.UserId == userId && v.PostId == postId);

            if (existingVote != null)
            {
                // If vote exists, update it
                existingVote.VoteType = voteType;
                _context.Votes.Update(existingVote);
            }
            else
            {
                // If no vote exists, add a new one
                var newVote = new Votes
                {
                    UserId = userId,
                    PostId = postId,
                    VoteType = voteType
                };
                await _context.Votes.AddAsync(newVote);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // New method to remove a vote
        public async Task<bool> RemoveVoteAsync(int userId, int postId)
        {
            var existingVote = await _context.Votes
                .FirstOrDefaultAsync(v => v.UserId == userId && v.PostId == postId);

            if (existingVote != null)
            {
                _context.Votes.Remove(existingVote);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
