using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SHCA.App.OrderProcessing.Monitor.Interfaces;
using SHCA.Domain.Entities;
using SHCA.Infra.Repositories;

namespace SHCA.App.OrderProcessing.Monitor.Process
{
    public class OrderProcessor : IOrderProcessor
    {
        private readonly ILogger log;
        private readonly AlertService alertService;
        private readonly OrderUpdateService orderUpdater;

        public OrderProcessor(ILogger logger, AlertService alertService, OrderUpdateService orderUpdater)
        {
            log = logger;
            this.alertService = alertService;
            this.orderUpdater = orderUpdater;
        }

        public async Task ProcessOrdersData(string ordersData)
        {
            List<Order>? orders;

            try
            {
                orders = JsonConvert.DeserializeObject<List<Order>>(ordersData);
            }
            catch (JsonException ex)
            {
                log.LogError(ex, "Failed to deserialize orders data.");
                return;
            }

            if (orders == null)
            {
                log.LogError("Orders data is null after deserialization.");
                return;
            }

            log.LogInformation("Processing orders data...");

            foreach (var order in orders)
            {
                try
                {
                    var updatedOrder = await ProcessOrder(order);
                    if (updatedOrder != null)
                    {
                        await orderUpdater.SendAlertAndUpdateOrder(updatedOrder);
                    }
                }
                catch (Exception ex)
                {
                    log.LogError(ex, $"Error processing order ID: {order.OrderId}");
                }
            }

            log.LogInformation("Orders data processed successfully.");
        }

        private async Task<Order?> ProcessOrder(Order order)
        {
            if (order?.Items?.Any() != true)
            {
                log.LogWarning("Order ID: {OrderId} has no items.", order?.OrderId);
                return order;
            }

            foreach (var item in order.Items)
            {
                try
                {
                    if (await item.IsItemDelivered())
                    {
                        if (order.OrderId.HasValue)
                        {
                            await alertService.SendAlertMessage(item, order.OrderId.Value.ToString());
                            await item.IncrementDeliveryNotification();
                        }
                        else
                        {
                            log.LogWarning("Order ID is null for an order with delivered items.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.LogError(ex, $"Error processing item in order ID: {order.OrderId}");
                }
            }

            return order;
        }
    }
}
