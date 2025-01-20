using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.API.Resources.VoteResources;
using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.Application.Interfaces;
using SocialMediaAPI.domain.entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly IVotesService _votesService;
        private readonly IMapper _mapper;
        private readonly IPostsRepository _postService;

        public VotesController(IVotesService votesService, IMapper mapper, IPostsRepository postService)
        {
            _votesService = votesService;
            _mapper = mapper;
            _postService = postService;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoteById(int id)
        {
            var vote = await _votesService.GetVoteByIdAsync(id);
            if (vote == null)
                return NotFound("Vote not found.");

            return Ok(vote);
        }

        [Authorize]
        [HttpGet("Post/{postId}")]
        public async Task<IActionResult> GetVotesByPostId(int postId)
        {
            var votes = await _votesService.GetVotesByPostIdAsync(postId);
            var voteResources = _mapper.Map<IEnumerable<VoteResource>>(votes);

            if (voteResources == null || !voteResources.Any())
            {
                return NotFound(new { message = "No votes found for the given post." });
            }

            return Ok(voteResources);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddVote([FromBody] CreateVoteResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserIdFromToken();
            if (userId == 0)
                return Unauthorized("User is not authenticated.");

            var post = await _votesService.GetPostByIdAsync(resource.PostId);
            if (post == null)
                return NotFound("Post not found.");

            var currentVoteStatus = await _votesService.GetVoteStatusAsync(userId, resource.PostId);

            // If user is upvoting and already has an upvote, remove the upvote
            if (currentVoteStatus == "Upvote" && resource.VoteType == "Upvote")
            {
                post.UpvotesCount--;
                await _votesService.RemoveVoteAsync(userId, resource.PostId); // Remove the existing upvote
            }
            // If user is downvoting and already has a downvote, remove the downvote
            else if (currentVoteStatus == "Downvote" && resource.VoteType == "Downvote")
            {
                post.DownvotesCount--;
                await _votesService.RemoveVoteAsync(userId, resource.PostId); // Remove the existing downvote
            }
            else if (currentVoteStatus != null && currentVoteStatus != resource.VoteType)
            {
                // If vote status is different, update the vote
                if (currentVoteStatus == "Upvote")
                    post.UpvotesCount--;
                else if (currentVoteStatus == "Downvote")
                    post.DownvotesCount--;

                if (resource.VoteType == "Upvote")
                    post.UpvotesCount++;
                else if (resource.VoteType == "Downvote")
                    post.DownvotesCount++;

                await _votesService.HandleVoteAsync(userId, resource.PostId, resource.VoteType);
            }
            else if (currentVoteStatus == null)
            {
                // If no existing vote, create a new one
                if (resource.VoteType == "Upvote" || resource.VoteType == "Downvote")
                {
                    var vote = _mapper.Map<Votes>(resource);
                    vote.UserId = userId;
                    vote.PostId = resource.PostId;

                    if (resource.VoteType == "Upvote")
                        post.UpvotesCount++;
                    else if (resource.VoteType == "Downvote")
                        post.DownvotesCount++;

                    await _votesService.AddVoteAsync(vote);
                }
                else
                {
                    return BadRequest("Invalid vote type.");
                }
            }

            await _votesService.UpdatePostAsync(post);

            return Ok("Vote successfully recorded.");
        }


        // Helper function to get user ID from token
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVote(int id)
        {
            var vote = await _votesService.GetVoteByIdAsync(id);
            if (vote == null)
                return NotFound("Vote not found.");

            var post = await _votesService.GetPostByIdAsync(vote.PostId);
            if (post == null)
                return NotFound("Associated post not found.");

            // Adjust the post vote counts accordingly
            if (vote.VoteType == "Upvote")
                post.UpvotesCount--;
            else if (vote.VoteType == "Downvote")
                post.DownvotesCount--;

            await _votesService.UpdatePostAsync(post);

            await _votesService.DeleteVoteAsync(id);

            return NoContent();
        }
    }
}