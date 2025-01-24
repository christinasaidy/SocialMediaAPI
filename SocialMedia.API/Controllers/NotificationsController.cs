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
        private readonly IUsersService _usersService; 
        private readonly IMapper _mapper;

        public NotificationsController(INotificationsService notificationsService, IUsersService usersService, IMapper mapper)
        {
            _notificationsService = notificationsService;
            _usersService = usersService;
            _mapper = mapper;
        }

      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var notification = await _notificationsService.GetNotificationByIdAsync(id);
            if (notification == null)
                return NotFound();
            return Ok(notification);
        }

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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNotification([FromBody] CreateNotificationResource resource)
        {
       
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           
            var SenderId = GetUserIdFromToken();
            if (SenderId == 0)
            {
                return Unauthorized("User is not authenticated.");
            }
        
            var notification = _mapper.Map<Notifications>(resource);
            notification.SenderID = SenderId;
            notification.CreatedAt = DateTime.UtcNow;

    
            var createdNotification = await _notificationsService.AddNotificationAsync(notification);

            return CreatedAtAction(nameof(GetNotificationById), new { id = createdNotification.Id }, createdNotification);
        }

     
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

            if (notification.SenderID != userId)
            {
                return Unauthorized("You are not authorized to delete this notification.");
            }

            await _notificationsService.DeleteNotificationAsync(id);
            return NoContent();
        }

        [Authorize]
        [HttpPatch("{id}/mark-as-read")]
        public async Task<IActionResult> MarkAsRead(int id)
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

            if (notification.ReceiverID != userId)
            {
                return Unauthorized("You are not authorized to update this notification.");
            }

            await _notificationsService.MarkAsReadAsync(id);
            return NoContent();
        }


        private int GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
    }
}
