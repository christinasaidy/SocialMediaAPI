using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.API.Resources;
using SocialMedia.API.Resources.PostResources;
using SocialMedia.API.Resources.UserResources;
using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using SocialMediaAPI.infrastructure.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    public UsersController(IUsersService usersService, IMapper mapper, IConfiguration configuration)
    {
        _usersService = usersService;
        _mapper = mapper;
        _configuration = configuration;
    }

    [HttpPost("signin")]
    [AllowAnonymous] // Explicitly allow unauthenticated users
    public async Task<IActionResult> SignIn([FromBody] SignInResource signInResource)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = _mapper.Map<Users>(signInResource);

        // Retrieve the user from the database
        var userFromDb = await _usersService.GetUserByUsernameAsync(user.UserName);

        // Check if the user exists and the password matches
        if (userFromDb == null || !BCrypt.Net.BCrypt.Verify(user.Password, userFromDb.Password))
            return Unauthorized("Invalid username or password");

        // Generate JWT token here
        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, userFromDb.UserName), // Add Username as a claim
        new Claim("UserId", userFromDb.Id.ToString()), // Add UserId as a custom claim
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "http://localhost:5128",
            audience: "http://localhost:3000",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(5),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.WriteToken(token);

        // Return the token
        return Ok(new { Token = jwtToken });
    }

    [HttpPost("signup")]
    [AllowAnonymous] // Explicitly allow unauthenticated users
    public async Task<IActionResult> SignUp([FromBody] SignUpResource signUpResource)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = _mapper.Map<Users>(signUpResource);

        // Check if the username already exists
        var existingUser = await _usersService.GetUserByUsernameAsync(user.UserName);
        if (existingUser != null)
            return Conflict("Username already exists");

        // Hash the user's password before saving it
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        // Create the new user
        var createdUser = await _usersService.CreateUserAsync(user);

        // Generate JWT token
        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, createdUser.UserName), // Add Username as a claim
        new Claim("UserId", createdUser.Id.ToString()), // Add UserId as a custom claim
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "http://localhost:5128",
            audience: "http://localhost:3000",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(5),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.WriteToken(token);

        // Return the created user along with the JWT token
        return Ok(new
        {
            Message = "User created successfully.",
            Token = jwtToken
        });
    }



    // Fetch User Profile (Requires Authentication)
    [Authorize] // Ensure only authenticated users can access this
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserProfile(int id)
    {
        var user = await _usersService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }
    [Authorize] // Requires authentication
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAccount([FromBody] DeleteUserResource request)
    {
        // Extract the user ID from the token
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized("Invalid token or user ID not found.");
        }

        if (string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Password is required.");
        }

        // Call the service layer to delete the user
        var isDeleted = await _usersService.DeleteUserAsync(userId, request.Password);

        if (!isDeleted)
        {
            return NotFound("User not found or already deleted.");
        }

        return Ok("User account deleted successfully.");
    }
    [Authorize] // Requires authentication
    [HttpGet("username")]
    public async Task<IActionResult> GetUsernameAsync()
    {
        // Extract the user ID from the claims (JWT token)
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized("Invalid token or user ID not found.");
        }

        // Fetch the username using the userId
        var username = await _usersService.GetUsernameByIdAsync(userId);

        if (username == null)
            return NotFound("Username not found.");

        return Ok(new { Username = username });
    }

    [Authorize] // Requires authentication
    [HttpGet("email")]
    public async Task<IActionResult> GetEmailAsync()
    {
        // Extract the user ID from the claims (JWT token)
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized("Invalid token or user ID not found.");
        }

        // Fetch the username using the userId
        var email = await _usersService.GetEmailByIdAsync(userId);

        if (email == null)
            return NotFound("Email not found.");

        return Ok(new { Email = email });
    }

    [Authorize]
    [HttpGet("posts")]
    public async Task<IActionResult> GetPosts() //gets the signed in user posts
    {
        // Extract the user ID from the token
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized("Invalid token or user ID not found.");
        }

        // Fetch posts using the service layer
        var posts = await _usersService.GetPostsByUserIdAsync(userId);

        if (posts == null || !posts.Any())
        {
            // Return an empty JSON array to avoid frontend parsing issues
            return Ok(new List<PostResource>());
        }

        // Map to PostResource
        var postResources = _mapper.Map<IEnumerable<PostResource>>(posts);

        return Ok(postResources);
    }


    [Authorize] // Requires authentication
    [HttpGet("bio")]
    public async Task<IActionResult> GetBio()
    {
        // Extract the user ID from the JWT token claims
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized("Invalid token or user ID not found.");
        }

        // Get the bio using the service
        var bio = await _usersService.GetBioByIdAsync(userId);

        if (bio == null)
        {
            return NotFound("Bio not found for the user.");
        }

        return Ok(new { Bio = bio });
    }

    [Authorize] // Requires authentication
    [HttpPost("addbio")]
    public async Task<IActionResult> AddBio([FromBody] string bio)
    {
        if (string.IsNullOrEmpty(bio))
        {
            return BadRequest("Bio cannot be empty.");
        }

        // Extract the user ID from the JWT token claims
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized("Invalid token or user ID not found.");
        }

        // Update the bio using the service
        var isUpdated = await _usersService.AddBioAsync(userId, bio);

        if (!isUpdated)
        {
            return NotFound("User not found or unable to update bio.");
        }

        return Ok("Bio updated successfully.");
    }

    [Authorize]
    [HttpGet("profile-picture")]
    public async Task<IActionResult> GetProfilePicture()
    {
        // Extract the user ID from the JWT token claims
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized("Invalid token or user ID not found.");
        }

        // Get the profile picture URL from the service layer
        var profilePictureUrl = await _usersService.GetProfilePictureByIdAsync(userId);

        if (string.IsNullOrEmpty(profilePictureUrl))
        {
            return NotFound("Profile picture not found.");
        }

        return Ok(new { ProfilePictureUrl = profilePictureUrl });
    }

    [Authorize]
    [HttpPost("profile-picture")]
    public async Task<IActionResult> UpdateProfilePicture([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(fileExtension))
        {
            return BadRequest("Invalid file type.");
        }

        const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        if (file.Length > MaxFileSize)
        {
            return BadRequest("File size exceeds the maximum limit.");
        }

        // Extract the user ID from the JWT token claims
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized("Invalid token or user ID not found.");
        }

        // Ensure the directory exists
        var directoryPath = Path.Combine("wwwroot", "profile-pictures");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Generate a unique file name
        var uniqueFileName = $"{userId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(directoryPath, uniqueFileName);

        try
        {
            // Save the file to the file system
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Update the user's profile picture URL in the database
            var profilePictureUrl = $"/profile-pictures/{uniqueFileName}";
            var isUpdated = await _usersService.AddProfilePictureAsync(userId, profilePictureUrl);

            if (!isUpdated)
            {
                return NotFound("User not found or unable to update profile picture.");
            }

            return Ok(new { ProfilePictureUrl = profilePictureUrl });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error while processing the file.");
        }
    }
    [Authorize] // Requires authentication
    [HttpGet("profile-picture/{id}")]
    public async Task<IActionResult> GetProfilePictureById(int id)
    {
        // Fetch the profile picture URL using the service
        var profilePictureUrl = await _usersService.GetProfilePictureByIdAsync(id);

        if (string.IsNullOrEmpty(profilePictureUrl))
        {
            return NotFound("Profile picture not found.");
        }

        return Ok(new { ProfilePictureUrl = profilePictureUrl });
    }

    [Authorize] // Requires authentication
    [HttpGet("{userId}/activity")]
    public async Task<IActionResult> GetUserActivity(int userId)
    {
        // Extract the authenticated user's ID from the token
        var authenticatedUserIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(authenticatedUserIdClaim) || !int.TryParse(authenticatedUserIdClaim, out int authenticatedUserId))
        {
            return Unauthorized("Invalid token or user ID not found.");
        }

        // Ensure the authenticated user is requesting their own activity
        if (authenticatedUserId != userId)
        {
            return Forbid("You are not authorized to view this user's activity.");
        }

        // Fetch counts using the service layer
        var postCount = await _usersService.GetPostCountByUserIdAsync(userId);
        var commentCount = await _usersService.GetCommentCountByUserIdAsync(userId);
        var engagementCount = await _usersService.GetEngagementCountByUserIdAsync(userId);

        // Calculate reputation points
        const int postWeight = 10; // 10 points per post
        const int commentWeight = 5; // 5 points per comment
        const int engagementWeight = 2; // 2 points per engagement

        int reputationPoints = (postCount * postWeight) + (commentCount * commentWeight) + (engagementCount * engagementWeight);

        // Update the user's reputationPoints in the database
        var user = await _usersService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        user.ReputationPoints = reputationPoints; // Update the reputationPoints field
        await _usersService.UpdateUserAsync(user); // Save changes to the database

        // Return the counts and calculated reputation points
        return Ok(new
        {
            PostCount = postCount,
            CommentCount = commentCount,
            EngagementCount = engagementCount,
            ReputationPoints = reputationPoints
        });
    }

    [Authorize]
    [HttpPatch("update-username")]
    public async Task<IActionResult> PatchUsernameAsync([FromBody] string newUsername)
    {
        if (string.IsNullOrEmpty(newUsername))
        {
            return BadRequest("Username cannot be empty.");
        }

        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized("Invalid token or user ID not found.");
        }

        var isUpdated = await _usersService.PatchUsernameAsync(userId, newUsername);

        if (!isUpdated)
        {
            return NotFound("User already exists, use another username.");
        }

        return Ok("Username updated successfully.");
    }

    [Authorize]
    [HttpPatch("update-email")]
    public async Task<IActionResult> PatchEmailAsync([FromBody] string newEmail)
    {
        if (string.IsNullOrEmpty(newEmail))
        {
            return BadRequest("Email cannot be empty.");
        }

        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized("Invalid token or user ID not found.");
        }

        var isUpdated = await _usersService.PatchEmailAsync(userId, newEmail);

        if (!isUpdated)
        {
            return NotFound("Email already is taken.");
        }

        return Ok("Email updated successfully.");
    }

    [Authorize]
    [HttpPatch("update-password")]
    public async Task<IActionResult> PatchPasswordAsync([FromBody] UpdatePasswordRequest dto)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized("Invalid token or user ID not found.");
        }

        if (dto.NewPassword != dto.ConfirmPassword)
        {
            return BadRequest("New password and confirmation password do not match.");
        }

        var result = await _usersService.PatchPasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);
        if (!result)
        {
            return BadRequest("Password update failed. Please check your current password.");
        }

        return Ok("Password updated successfully.");
    }


}






