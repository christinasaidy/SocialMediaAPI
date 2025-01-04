using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.API.Resources.CommentResources;
using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using AutoMapper;
using System.Threading.Tasks;

namespace SocialMedia.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentsService;
        private readonly IMapper _mapper;

        public CommentsController(ICommentsService commentsService, IMapper mapper)
        {
            _commentsService = commentsService;
            _mapper = mapper;
        }

        // GET: api/Comments/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _commentsService.GetCommentByIdAsync(id);
            if (comment == null)
                return NotFound("Comment not found.");

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
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserIdFromToken();
            if (userId == 0)
                return Unauthorized("User is not authenticated.");

            var post = await _commentsService.GetPostByIdAsync(resource.PostId);
            if (post == null)
                return NotFound("Post not found.");

            var comment = _mapper.Map<Comments>(resource);

           
            comment.UserId = userId;
            comment.PostId = resource.PostId;
            comment.CreatedAt = DateTime.UtcNow;
            comment.Post = post;  
            comment.User = await _commentsService.GetUserByIdAsync(userId);  

           
            var createdComment = await _commentsService.AddCommentAsync(comment);

            return CreatedAtAction(nameof(GetCommentById), new { id = createdComment.Id }, createdComment);
        }


        // PUT: api/Comments/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserIdFromToken();
            if (userId == 0)
                return Unauthorized("User is not authenticated.");

            var existingComment = await _commentsService.GetCommentByIdAsync(id);
            if (existingComment == null)
                return NotFound("Comment not found.");

            if (existingComment.UserId != userId)
                return Unauthorized("You are not authorized to update this comment.");

            // Map resource to existing comment
            _mapper.Map(resource, existingComment);

            var updatedComment = await _commentsService.UpdateCommentAsync(existingComment);

            return Ok(updatedComment);
        }

        // DELETE: api/Comments/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == 0)
                return Unauthorized("User is not authenticated.");

            var comment = await _commentsService.GetCommentByIdAsync(id);
            if (comment == null)
                return NotFound("Comment not found.");

            if (comment.UserId != userId)
                return Unauthorized("You are not authorized to delete this comment.");

            await _commentsService.DeleteCommentAsync(id);
            return NoContent();
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
    }
}
