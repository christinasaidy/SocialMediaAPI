using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaAPI.domain.entities;
using SocialMediaAPI.application.Interfaces;

namespace SocialMediaAPI.application.Services
{
    public class VotesService : IVotesService
    {
        private readonly IVotesRepository _votesRepository;

        public VotesService(IVotesRepository votesRepository)
        {
            _votesRepository = votesRepository;
        }

        public async Task<Votes> GetVoteByIdAsync(int id)
        {
            return await _votesRepository.GetVoteByIdAsync(id);
        }

        public async Task<IEnumerable<Votes>> GetVotesByPostIdAsync(int postId)
        {
            return await _votesRepository.GetVotesByPostIdAsync(postId);
        }

        public async Task<IEnumerable<Votes>> GetVotesByUserIdAsync(int userId)
        {
            return await _votesRepository.GetVotesByUserIdAsync(userId);
        }

        public async Task<Votes> AddVoteAsync(Votes vote)
        {
            return await _votesRepository.AddVoteAsync(vote);
        }

        public async Task<Votes> UpdateVoteAsync(Votes vote)
        {
            return await _votesRepository.UpdateVoteAsync(vote);
        }

        public async Task DeleteVoteAsync(int id)
        {
            await _votesRepository.DeleteVoteAsync(id);
        }
    }
}
