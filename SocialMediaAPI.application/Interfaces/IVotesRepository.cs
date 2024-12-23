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
    }
}
