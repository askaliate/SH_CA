using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SHCA.App.OrderProcessing.Monitor.Process;


namespace SHCA.Api.OrderProcessing.Monitor.Controllers
{
    public class OrderMonitorController : ControllerBase
    {
        private readonly OrderStatusProcessorService _orderStatusProcessorService;

        public OrderMonitorController(OrderStatusProcessorService orderStatusProcessorService)
        {
            _orderStatusProcessorService = orderStatusProcessorService;
        }

        [HttpPost("process-orders")]
        public async Task<IActionResult> ProcessOrders()
        {
            await _orderStatusProcessorService.ProcessOrders();
            return Ok("Orders processed successfully.");
        }
    }
}
