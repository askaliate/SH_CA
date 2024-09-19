using SHCA.Domain.Entities;
using SHCA.Domain.Interfaces.Repositories;
using SHCA.Infra.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCA.App.OrderProcessing.Monitor.Notification
{
    public class OrderNotificationService
    {
        private readonly IOrderNotificationRepository _repository;

        public OrderNotificationService(IOrderNotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<OrderNotification>> GetAllNotificationsAsync()
        {
            return await _repository.GetAllNotificationsAsync();
        }
    }
}
