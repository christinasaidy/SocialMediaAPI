using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.API.Resources.UserResources;
using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;
    private readonly IMapper _mapper;

    public UsersController(IUsersService usersService, IMapper mapper)
    {
        _usersService = usersService;
        _mapper = mapper;
    }

    [HttpPost("signin")]
    [AllowAnonymous] // Explicitly allow unauthenticated users
    public async Task<IActionResult> SignIn([FromBody] SignInResource signInResource)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = _mapper.Map<Users>(signInResource);

        var userFromDb = await _usersService.GetUserByUsernameAsync(user.UserName);

        if (userFromDb == null || userFromDb.Password != user.Password)
            return Unauthorized("Invalid username or password");

        // Generate JWT token here
        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, userFromDb.UserName), // Add Username as a claim
        new Claim("UserId", userFromDb.Id.ToString()), // Add UserId as a custom claim
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKey12345111111111")); 
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "http://localhost:5128", 
            audience: "http://localhost:3000", 
            claims: claims,
            expires: DateTime.UtcNow.AddHours(5), 
            signingCredentials: credentials
        );
        Console.WriteLine($"Token expiration: {token.ValidTo}");


        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.WriteToken(token); // Serialize the JWT token
        Console.WriteLine("Generated Token: " + jwtToken);

        //return token 
        return Ok(new { Token = jwtToken });
    }


    [HttpPost("signup")]
    [AllowAnonymous] // Explicitly allow unauthenticated users
    public async Task<IActionResult> SignUp([FromBody] SignUpResource signUpResource)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = _mapper.Map<Users>(signUpResource);

        var existingUser = await _usersService.GetUserByUsernameAsync(user.UserName);
        if (existingUser != null)
            return Conflict("Username already exists");

        var createdUser = await _usersService.CreateUserAsync(user);

        return CreatedAtAction(nameof(SignIn), new { id = createdUser.Id }, createdUser);
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
    public async Task<IActionResult> DeleteAccount()
    {
        // Extract the user ID from the token
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized("Invalid token or user ID not found.");
        }

        // Call the service layer to delete the user
        var isDeleted = await _usersService.DeleteUserAsync(userId);

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

}
