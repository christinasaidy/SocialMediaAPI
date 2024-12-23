using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using Microsoft.Extensions.Hosting;

namespace SocialMedia.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        // GET: api/Comments/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _commentsService.GetCommentByIdAsync(id);
            if (comment == null)
                return NotFound();
            return Ok(comment);
        }

        // GET: api/Comments/Post/{postId}
        [HttpGet("Post/{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(int postId)
        {
            var comments = await _commentsService.GetCommentsByPostIdAsync(postId);
            return Ok(comments);
        }

        // POST: api/Comments
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] Comments comment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdComment = await _commentsService.AddCommentAsync(comment);
            return CreatedAtAction(nameof(GetCommentById), new { id = createdComment.Id }, createdComment);
        }

        // PUT: api/Comments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] Comments comment)
        {
            if (id != comment.Id)
                return BadRequest("Comment ID mismatch");

            var updatedComment = await _commentsService.UpdateCommentAsync(comment);
            if (updatedComment == null)
                return NotFound();

            return Ok(updatedComment);
        }

        // DELETE: api/Comments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _commentsService.DeleteCommentAsync(id);
            return NoContent();
        }
    }
}
