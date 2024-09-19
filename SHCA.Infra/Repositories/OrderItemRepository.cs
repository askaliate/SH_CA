using Microsoft.EntityFrameworkCore;
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
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApiDbContext _context;

        public OrderItemRepository(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync()
        {
            return await _context.OrderItems.ToListAsync();
        }
    }

    public static class OrderItemExtensions
    {
        public static Task<bool> IsItemDelivered(this OrderItem item)
        {
            // In a production environment, this string would pull from an azure configuration - see tests for mock of this
            bool result = item.Status != null && item.Status.Equals("Delivered", StringComparison.OrdinalIgnoreCase);
            return Task.FromResult(result);
        }

        public static Task IncrementDeliveryNotification(this OrderItem item)
        {
            // In a production environment, this string would pull from an azure configuration - see tests for mock of this
            item.DeliveryNotificationCount++;
            return Task.CompletedTask;
        }
    }
}
