using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.API.Resources.PostResources;
using SocialMediaAPI.application.Interfaces;
using System.Security.Claims;


namespace SocialMedia.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostsService _postsService;
        
        //##! here is where the connection between user, category and post is made
        private readonly ICategoriesService _categoryService; // Service to get category details
        private readonly IUsersService _userService; // Service to get user details


        private readonly IMapper _mapper; 

        public PostsController(IPostsService postsService, ICategoriesService categoryService, IUsersService userService, IMapper mapper)
        {
            _postsService = postsService;
            _categoryService = categoryService;
            _userService = userService;
            _mapper = mapper;
        }

        // POST: api/Posts
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostResource postCreateDto)
        {

            // Get the userId from the JWT token (authentication required)
            var userId = GetUserIdFromToken(); 
            Console.WriteLine($"User ID: {userId}");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            if (userId == 0)
            {
                return Unauthorized("User is not authenticated.");
            }

            // Get the CategoryId from the category name
            var category = await _categoryService.GetCategoryByNameAsync(postCreateDto.CategoryName);

            if (category == null)
            {
                return NotFound($"Category '{postCreateDto.CategoryName}' not found.");
            }

            // Get user details
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Use AutoMapper to map from CreatePostResource to Post
            var post = _mapper.Map<Posts>(postCreateDto);

            // Set the CategoryId and UserId manually
            post.CategoryId = category.Id;
            post.UserId = user.Id;
            post.CreatedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;
            post.Author = user; // Use the user object retrieved from the user service
            post.Category = category; // Set the category object

            // Save the post using the service
            var createdPost = await _postsService.AddPostAsync(post);

            // Return the created post with status code 201 (Created)
            return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, createdPost);
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

        // Helper method to get UserId from the JWT token
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId"); 
            Console.WriteLine($"User ID Claim: {userIdClaim?.Value}");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
    }
}
