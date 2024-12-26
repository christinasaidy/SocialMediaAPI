using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.application.Interfaces;
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

        public VotesController(IVotesService votesService)
        {
            _votesService = votesService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoteById(int id)
        {
            var vote = await _votesService.GetVoteByIdAsync(id);
            if (vote == null)
                return NotFound();
            return Ok(vote);
        }

    
        [HttpGet("Post/{postId}")]
        public async Task<IActionResult> GetVotesByPostId(int postId)
        {
            var votes = await _votesService.GetVotesByPostIdAsync(postId);
            return Ok(votes);
        }

        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetVotesByUserId(int userId)
        {
            var votes = await _votesService.GetVotesByUserIdAsync(userId);
            return Ok(votes);
        }

     
        [HttpPost]
        public async Task<IActionResult> AddVote([FromBody] Votes vote)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdVote = await _votesService.AddVoteAsync(vote);
            return CreatedAtAction(nameof(GetVoteById), new { id = createdVote.Id }, createdVote);
        }

     
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVote(int id, [FromBody] Votes vote)
        {
            if (id != vote.Id)
                return BadRequest("Vote ID mismatch");

            var updatedVote = await _votesService.UpdateVoteAsync(vote);
            if (updatedVote == null)
                return NotFound();

            return Ok(updatedVote);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVote(int id)
        {
            await _votesService.DeleteVoteAsync(id);
            return NoContent();
        }
    }
}
