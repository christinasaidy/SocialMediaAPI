using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using SocialMediaAPI.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaAPI.infrastructure.Repositories
{
    public class NotificationsRepository : INotificationsRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Notifications> GetNotificationByIdAsync(int id)
        {
            return await _context.Notifications
                .Include(n => n.Recipient)  
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<IEnumerable<Notifications>> GetAllNotificationsAsync()
        {
            return await _context.Notifications
                .Include(n => n.Recipient) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Notifications>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .Include(n => n.Recipient) 
                .ToListAsync();
        }

        public async Task<Notifications> AddNotificationAsync(Notifications notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<Notifications> UpdateNotificationAsync(Notifications notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task DeleteNotificationAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }
    }
}
