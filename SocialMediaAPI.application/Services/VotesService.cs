using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaAPI.domain.entities;
using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.Application.Interfaces;

namespace SocialMediaAPI.application.Services
{
    public class VotesService : IVotesService
    {
        private readonly IVotesRepository _votesRepository;
        private readonly IPostsRepository _postService;
        private readonly IUsersService _userService;
        private readonly IPostsRepository _postRepository;

        public VotesService(IVotesRepository votesRepository, IPostsRepository postService, IUsersService userService, IPostsRepository postRepository)
        {
            _votesRepository = votesRepository;
            _postService = postService;
            _userService = userService;
            _postRepository = postRepository;
        }

        public async Task<Votes> GetVoteByIdAsync(int id)
        {
            return await _votesRepository.GetVoteByIdAsync(id);
        }

        public async Task<Posts> GetPostByIdAsync(int postId)
        {
            return await _postService.GetPostByIdAsync(postId);
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

        public async Task<Users> GetUserByIdAsync(int userId)
        {
            return await _userService.GetUserByIdAsync(userId);
        }

        public async Task<Votes> GetVoteByUserAndPostAsync(int userId, int postId)
        {
            return await _votesRepository.GetVoteByUserAndPostAsync(userId, postId);
        }

        public async Task UpdatePostAsync(Posts post)
        {
            await _postRepository.UpdatePostAsync(post);
        }


        // Get the current vote status of a user for a given post
        public async Task<string> GetVoteStatusAsync(int userId, int postId)
        {
            var vote = await _votesRepository.GetVoteByUserAndPostAsync(userId, postId);
            return vote?.VoteType; // Return VoteType (null if no vote)
        }

        // Handle adding/updating a vote
        public async Task<bool> HandleVoteAsync(int userId, int postId, string voteType)
        {
            var existingVote = await _votesRepository.GetVoteByUserAndPostAsync(userId, postId);

            if (existingVote != null)
            {
                // Update the existing vote
                existingVote.VoteType = voteType;
                await _votesRepository.UpdateVoteAsync(existingVote);
            }
            else
            {
                // Add a new vote
                var newVote = new Votes
                {
                    PostId = postId,
                    UserId = userId,
                    VoteType = voteType
                };
                await _votesRepository.AddVoteAsync(newVote);
            }
            return true;
        }

        public async Task<bool> RemoveVoteAsync(int userId, int postId)
        {
            var existingVote = await _votesRepository.GetVoteByUserAndPostAsync(userId, postId);

            if (existingVote != null)
            {
                await _votesRepository.DeleteVoteAsync(existingVote.Id);
                return true;
            }

            return false;
        }
    }
}
