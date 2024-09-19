using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SHCA.App.OrderProcessing.Monitor.Notification;
using SHCA.Domain.Entities;

namespace SHCA.Api.Order.Monitor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderNotificationController : ControllerBase
    {
        private readonly OrderNotificationService _notificationService;

        public OrderNotificationController(OrderNotificationService service)
        {
            _notificationService = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderNotification>>> GetAllNotifications()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            return Ok(notifications);
        }

        // Other action methods for adding, updating, and deleting notifications
    }
}

