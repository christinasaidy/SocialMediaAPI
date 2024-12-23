using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.application.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly INotificationsRepository _notificationsRepository;

        public NotificationsService(INotificationsRepository notificationsRepository)
        {
            _notificationsRepository = notificationsRepository;
        }

        public async Task<Notifications> GetNotificationByIdAsync(int id)
        {
            return await _notificationsRepository.GetNotificationByIdAsync(id);
        }

        public async Task<IEnumerable<Notifications>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _notificationsRepository.GetNotificationsByUserIdAsync(userId);
        }

        public async Task<Notifications> AddNotificationAsync(Notifications notification)
        {
            return await _notificationsRepository.AddNotificationAsync(notification);
        }

        public async Task<Notifications> UpdateNotificationAsync(Notifications notification)
        {
            return await _notificationsRepository.UpdateNotificationAsync(notification);
        }

        public async Task DeleteNotificationAsync(int id)
        {
            await _notificationsRepository.DeleteNotificationAsync(id);
        }
    }
}
