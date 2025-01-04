using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.API.Resources.NotificationResources;
using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialMedia.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationsService;
        private readonly IUsersService _usersService; // To fetch user details
        private readonly IMapper _mapper;

        public NotificationsController(INotificationsService notificationsService, IUsersService usersService, IMapper mapper)
        {
            _notificationsService = notificationsService;
            _usersService = usersService;
            _mapper = mapper;
        }

        // GET: api/Notifications/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var notification = await _notificationsService.GetNotificationByIdAsync(id);
            if (notification == null)
                return NotFound();
            return Ok(notification);
        }

        // GET: api/Notifications/User/{userId}
        [Authorize]
        [HttpGet("User")]
        public async Task<IActionResult> GetNotificationsByUserId()
        {
            var userId = GetUserIdFromToken();

            if (userId == 0)
            {
                return Unauthorized("User is not authenticated.");
            }

            var notifications = await _notificationsService.GetNotificationsByUserIdAsync(userId);
            return Ok(notifications);
        }

        // POST: api/Notifications
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNotification([FromBody] CreateNotificationResource resource)
        {
            // Validate the model
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get the userId from the JWT token
            var userId = GetUserIdFromToken();
            if (userId == 0)
            {
                return Unauthorized("User is not authenticated.");
            }

            // Get the recipient user details (optional, if needed)
            var recipient = await _usersService.GetUserByIdAsync(userId);
            if (recipient == null)
            {
                return NotFound("Recipient user not found.");
            }

            // Map resource to notification entity
            var notification = _mapper.Map<Notifications>(resource);

            // Set additional fields
            notification.UserId = userId;
            notification.CreatedAt = DateTime.UtcNow;

            // Add the notification via the service
            var createdNotification = await _notificationsService.AddNotificationAsync(notification);

            // Return the created notification with status code 201 (Created)
            return CreatedAtAction(nameof(GetNotificationById), new { id = createdNotification.Id }, createdNotification);
        }

        // DELETE: api/Notifications/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var userId = GetUserIdFromToken();

            if (userId == 0)
            {
                return Unauthorized("User is not authenticated.");
            }

            var notification = await _notificationsService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return NotFound("Notification not found.");
            }

            if (notification.UserId != userId)
            {
                return Unauthorized("You are not authorized to delete this notification.");
            }

            await _notificationsService.DeleteNotificationAsync(id);
            return NoContent();
        }

        // Utility to get the userId from the JWT token
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
    }
}
