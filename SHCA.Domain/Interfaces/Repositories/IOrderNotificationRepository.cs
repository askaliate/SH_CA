using SHCA.Core.Interface;
using SHCA.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCA.Domain.Interfaces.Repositories
{
    public interface IOrderNotificationRepository
    {
        Task<IEnumerable<OrderNotification>> GetAllNotificationsAsync();
        Task<OrderNotification> GetNotificationByIdAsync(long id);
        Task AddNotificationAsync(OrderNotification notification);
        Task UpdateNotificationAsync(OrderNotification notification);
        Task DeleteNotificationAsync(long id);
    }
}
