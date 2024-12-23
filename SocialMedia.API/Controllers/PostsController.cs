using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;

namespace SocialMedia.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostsService _postsService;

        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
        }

        // GET: api/Posts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _postsService.GetPostByIdAsync(id);
            if (post == null)
                return NotFound();
            return Ok(post);
        }

        // GET: api/Posts/Category/{categoryId}
        [HttpGet("Category/{categoryId}")]
        public async Task<IActionResult> GetPostsByCategoryId(int categoryId)
        {
            var posts = await _postsService.GetPostsByCategoryIdAsync(categoryId);
            return Ok(posts);
        }

        // GET: api/Posts/User/{userId}
        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetPostsByUserId(int userId)
        {
            var posts = await _postsService.GetPostsByUserIdAsync(userId);
            return Ok(posts);
        }

        // POST: api/Posts
        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] Posts post)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPost = await _postsService.AddPostAsync(post);
            return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, createdPost);
        }

        // PUT: api/Posts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] Posts post)
        {
            if (id != post.Id)
                return BadRequest("Post ID mismatch");

            var updatedPost = await _postsService.UpdatePostAsync(post);
            if (updatedPost == null)
                return NotFound();

            return Ok(updatedPost);
        }

        // DELETE: api/Posts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postsService.DeletePostAsync(id);
            return NoContent();
        }
    }
}
