using Microsoft.EntityFrameworkCore;
using SHCA.Core.Interface;
using SHCA.Domain.Entities;
using SHCA.Domain.Interfaces.Repositories;
using SHCA.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCA.Infra.Repositories
{
    public class OrderNotificationRepository : IOrderNotificationRepository
    {
        private readonly Data.ApiDbContext _context;

        public OrderNotificationRepository(Data.ApiDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderNotification>> GetAllNotificationsAsync()
        {
            return await _context.OrderNotifications.ToListAsync();
        }

        public async Task<OrderNotification> GetNotificationByIdAsync(long id)
        {
            var notification = await _context.OrderNotifications.FindAsync(id);
            if (notification == null)
            {
                throw new KeyNotFoundException($"OrderNotification with ID {id} not found.");
            }
            return notification;
        }

        public async Task AddNotificationAsync(OrderNotification notification)
        {
            await _context.OrderNotifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNotificationAsync(OrderNotification notification)
        {
            _context.OrderNotifications.Update(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(long id)
        {
            var notification = await _context.OrderNotifications.FindAsync(id);
            if (notification != null)
            {
                _context.OrderNotifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }
    }
}
