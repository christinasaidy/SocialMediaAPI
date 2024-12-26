using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using Microsoft.Extensions.Hosting;

namespace SocialMedia.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _usersService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

      
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] Users user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdUser = await _usersService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Users user)
        {
            if (id != user.Id)
                return BadRequest("User ID mismatch");

            var updatedUser = await _usersService.UpdateUserAsync(user);
            if (updatedUser == null)
                return NotFound();

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var isDeleted = await _usersService.DeleteUserAsync(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
