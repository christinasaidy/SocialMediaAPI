using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.API.Resources.CommentResources;
using SocialMedia.API.Resources.VoteResources;
using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.application.Services;
using SocialMediaAPI.domain.entities;
using System.Collections.Generic;
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
        }//done

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoteById(int id)
        {
            var vote = await _votesService.GetVoteByIdAsync(id);
            if (vote == null)
                return NotFound("vote not found.");

            return Ok(vote);
        }//done


        [HttpGet("Post/{postId}")]
        public async Task<IActionResult> GetVotesByPostId(int postId)
        {
            var vote = await _votesService.GetVotesByPostIdAsync(postId);
            return Ok(vote);
        }//done
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

            var existingVote = await _votesService.GetVoteByUserAndPostAsync(userId, resource.PostId);
            if (existingVote != null)
            {
                if (existingVote.VoteType != resource.VoteType)
                {
                    if (existingVote.VoteType == "Upvote")
                        post.UpvotesCount--;
                    else if (existingVote.VoteType == "Downvote")
                        post.DownvotesCount--;

                    if (resource.VoteType == "Upvote")
                        post.UpvotesCount++;
                    else if (resource.VoteType == "Downvote")
                        post.DownvotesCount++;

                    existingVote.VoteType = resource.VoteType;
                    await _votesService.UpdateVoteAsync(existingVote);
                }
            }
            else
            {
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



        //helper function
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVote(int id)
        {
      
            var vote = await _votesService.GetVoteByIdAsync(id);
            if (vote == null)
                return NotFound("Vote not found.");

            var post = await _votesService.GetPostByIdAsync(vote.PostId);
            if (post == null)
                return NotFound("Associated post not found.");

        
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
