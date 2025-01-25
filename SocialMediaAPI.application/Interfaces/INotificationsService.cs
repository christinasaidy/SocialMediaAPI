using SocialMediaAPI.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.application.Interfaces
{
    public interface INotificationsService
    {
        Task<Notifications> GetNotificationByIdAsync(int id);
        Task<IEnumerable<Notifications>> GetNotificationsByUserIdAsync(int userId);
        Task<Notifications> AddNotificationAsync(Notifications notification);
        Task<Notifications> UpdateNotificationAsync(Notifications notification);
        Task DeleteNotificationAsync(int id);
        Task MarkAsReadAsync(int id);
        Task<int> GetUnreadNotificationsCountAsync(int userId);

    }
}
