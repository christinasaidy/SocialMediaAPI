using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationsService;

        public NotificationsController(INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
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
        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetNotificationsByUserId(int userId)
        {
            var notifications = await _notificationsService.GetNotificationsByUserIdAsync(userId);
            return Ok(notifications);
        }

        // POST: api/Notifications
        [HttpPost]
        public async Task<IActionResult> AddNotification([FromBody] Notifications notification)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdNotification = await _notificationsService.AddNotificationAsync(notification);
            return CreatedAtAction(nameof(GetNotificationById), new { id = createdNotification.Id }, createdNotification);
        }

        // PUT: api/Notifications/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(int id, [FromBody] Notifications notification)
        {
            if (id != notification.Id)
                return BadRequest("Notification ID mismatch");

            var updatedNotification = await _notificationsService.UpdateNotificationAsync(notification);
            if (updatedNotification == null)
                return NotFound();

            return Ok(updatedNotification);
        }

        // DELETE: api/Notifications/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            await _notificationsService.DeleteNotificationAsync(id);
            return NoContent();
        }
    }
}
