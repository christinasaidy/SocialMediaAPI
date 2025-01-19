using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaAPI.domain.entities;

namespace SocialMediaAPI.application.Interfaces
{
    public interface IVotesRepository
    {
        Task<Votes> GetVoteByIdAsync(int id);
        Task<IEnumerable<Votes>> GetVotesByPostIdAsync(int postId);
        Task<IEnumerable<Votes>> GetVotesByUserIdAsync(int userId);
        Task<Votes> AddVoteAsync(Votes vote);
        Task<Votes> UpdateVoteAsync(Votes vote);
        Task DeleteVoteAsync(int id);
        Task<Posts> GetPostByIdAsync(int postId);
        Task<Votes> GetVoteByUserAndPostAsync(int userId, int postId);

        // New methods
        Task<string> GetVoteStatusAsync(int userId, int postId); // Get the current vote status (upvoted/downvoted)
        Task<bool> HandleVoteAsync(int userId, int postId, string voteType); // Add or update a vote
        Task<bool> RemoveVoteAsync(int userId, int postId); // Optionally remove a user's vote
    }
}
