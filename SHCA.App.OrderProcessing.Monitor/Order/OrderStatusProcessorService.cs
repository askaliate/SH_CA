using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SHCA.App.OrderProcessing.Monitor.Interfaces;

namespace SHCA.App.OrderProcessing.Monitor.Process
{
    public class OrderStatusProcessorService
    {
        private readonly IOrderServiceClient orderFetcher;
        private readonly IOrderProcessor orderProcessor;
        private readonly ILogger<OrderStatusProcessorService> log;

        public OrderStatusProcessorService(IOrderServiceClient orderFetcher, IOrderProcessor orderProcessor, ILogger<OrderStatusProcessorService> logger)
        {
            this.orderFetcher = orderFetcher;
            this.orderProcessor = orderProcessor;
            this.log = logger;
        }

        public async Task ProcessOrders()
        {
            string? ordersData;

            try
            {
                ordersData = await orderFetcher.FetchMedicalEquipmentOrders();
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to fetch orders data.");
                return;
            }

            if (ordersData != null)
            {
                try
                {
                    await orderProcessor.ProcessOrdersData(ordersData);
                }
                catch (Exception ex)
                {
                    log.LogError(ex, "Failed to process orders data.");
                }
            }
            else
            {
                log.LogError("No orders data to process.");
            }
        }
    }
}
