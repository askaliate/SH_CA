using Microsoft.Identity.Client;
using SHCA.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SHCA.Domain.Entities;
using Newtonsoft.Json;
using SHCA.Infra.Repositories;
using SHCA.App.OrderProcessing.Monitor.Auth;
using SHCA.App.OrderProcessing.Monitor.Interfaces;

namespace SHCA.App.OrderProcessing.Monitor.Process
{
    public class OrderStatusProcessorService
    {
        private readonly IOrderServiceClient orderFetcher;
        private readonly IOrderProcessor orderProcessor;
        private readonly ILogger<OrderStatusProcessorService> log; // Add logger

        public OrderStatusProcessorService(IOrderServiceClient orderFetcher, IOrderProcessor orderProcessor, ILogger<OrderStatusProcessorService> logger)
        {
            this.orderFetcher = orderFetcher;
            this.orderProcessor = orderProcessor;
            this.log = logger; // Initialize logger
        }

        public async Task ProcessOrders()
        {
            var ordersData = await orderFetcher.FetchMedicalEquipmentOrders();

            if (ordersData != null)
            {
                await orderProcessor.ProcessOrdersData(ordersData);
            }
            else
            {
                log.LogError("No orders data to process."); // Use logger to log error
            }
        }
    }
}
