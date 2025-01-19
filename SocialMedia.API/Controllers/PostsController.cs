using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.API.Resources.PostResources;
using SocialMedia.API.Resources.CategoryResources;
using SocialMedia.API.Resources.UserResources;
using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using System.Security.Claims;
using SocialMediaAPI.Application.Interfaces;

namespace SocialMedia.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostsService _postsService;
        private readonly ICategoriesService _categoryService;
        private readonly IUsersService _userService;
        private readonly IMapper _mapper;

        public PostsController(IPostsService postsService, ICategoriesService categoryService, IUsersService userService, IMapper mapper)
        {
            _postsService = postsService;
            _categoryService = categoryService;
            _userService = userService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostResource postCreateDto)
        {
            var userId = GetUserIdFromToken();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (userId == 0)
            {
                return Unauthorized("User is not authenticated.");
            }

            var category = await _categoryService.GetCategoryByNameAsync(postCreateDto.CategoryName);

            if (category == null)
            {
                return NotFound($"Category '{postCreateDto.CategoryName}' not found.");
            }

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var post = _mapper.Map<Posts>(postCreateDto);

            post.CategoryId = category.Id;
            post.UserId = user.Id;
            post.CreatedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;
            post.Author = user;
            post.Category = category;

            var createdPost = await _postsService.AddPostAsync(post);

            var postResource = _mapper.Map<PostResource>(createdPost);
            return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, postResource);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _postsService.GetPostByIdAsync(id);
            if (post == null)
                return NotFound();

            var postResource = _mapper.Map<PostResource>(post);
            return Ok(postResource);
        }

        // New endpoint for retrieving images associated with a post
        [HttpGet("{id}/images")]
        public async Task<IActionResult> GetImagesByPostId(int id)
        {
            var images = await _postsService.GetImagesByPostIdAsync(id);
            if (images == null || !images.Any())
            {
                return NotFound("No images found for this post.");
            }

            return Ok(images);
        }

        [Authorize]
        [HttpPost("{id}/images")]
        public async Task<IActionResult> AddImagesToPost(int id, [FromForm] List<IFormFile> imageFiles)
        {
            // Validate the number of images
            if (imageFiles == null || imageFiles.Count > 4)
            {
                return BadRequest("You can upload a maximum of 4 images per post.");
            }

            var userId = GetUserIdFromToken();

            if (userId == 0)
            {
                return Unauthorized("User is not authenticated.");
            }

            var post = await _postsService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound("Post not found.");
            }

            if (post.UserId != userId)
            {
                return Unauthorized("You are not authorized to add images to this post.");
            }

            var postImages = new List<string>();

            // Process each uploaded image file
            foreach (var file in imageFiles)
            {
                if (file.Length > 0)
                {
                    // Generate a unique file name for the image (optional)
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "postpictures", fileName);

                    // Save the image to the file system
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Add the image path to the list (this could be stored in the database)
                    postImages.Add($"/postpictures/{fileName}");
                }
            }

            // Save the image paths to the post (optional step)
            if (postImages.Any())
            {
                await _postsService.AddImagesToPostAsync(id, postImages);
            }

            return NoContent();
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var userId = GetUserIdFromToken();

            if (userId == 0)
            {
                return Unauthorized("User is not authenticated.");
            }

            var post = await _postsService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound("Post not found.");
            }

            if (post.UserId != userId)
            {
                return Unauthorized("You are not authorized to delete this post.");
            }

            await _postsService.DeletePostAsync(id);
            return NoContent();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostResource postUpdateDto)
        {
            var userId = GetUserIdFromToken();

            if (userId == 0)
            {
                return Unauthorized("User is not authenticated.");
            }

            var post = await _postsService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound("Post not found.");
            }

            if (post.UserId != userId)
            {
                return Unauthorized("You are not authorized to update this post.");
            }

            _mapper.Map(postUpdateDto, post);
            post.UpdatedAt = DateTime.UtcNow;

            var updatedPost = await _postsService.UpdatePostAsync(post);

            if (updatedPost == null)
            {
                return BadRequest("Failed to update post.");
            }

            var updatedPostResource = _mapper.Map<PostResource>(updatedPost);
            return Ok(updatedPostResource);
        }

        [HttpGet("top")]
        public async Task<IActionResult> GetTopPostsByUpvotes([FromQuery] int count = 5)
        {
            var posts = await _postsService.GetTopPostsByUpvotesAsync(count);
            if (posts == null || !posts.Any())
            {
                return NotFound("No posts found.");
            }

            var postResources = _mapper.Map<IEnumerable<PostResource>>(posts);
            return Ok(postResources);
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestPosts([FromQuery] int count = 10, [FromQuery] int offset = 0)
        {
            var posts = await _postsService.GetLatestPostsAsync(count, offset);
            if (posts == null || !posts.Any())
            {
                return NotFound("No recent posts found.");
            }

            var postResources = _mapper.Map<IEnumerable<PostResource>>(posts);
            return Ok(postResources);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetTotalPostsCount()
        {
            var totalPostsCount = await _postsService.GetTotalPostsCountAsync();
            return Ok(totalPostsCount);
        }
    }
}
