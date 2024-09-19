using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SHCA.App.OrderProcessing.Monitor.Auth;
using SHCA.Domain.Entities;

namespace SHCA.App.OrderProcessing.Monitor.Process
{
    public class OrderUpdateService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger log;
        private readonly ITokenHandler tokenHandler;

        public OrderUpdateService(HttpClient httpClient, ILogger logger, ITokenHandler tokenHandler)
        {
            this.httpClient = httpClient;
            log = logger;
            this.tokenHandler = tokenHandler;
        }

        public async Task SendAlertAndUpdateOrder(Order order)
        {
            string updateApiUrl = "https://update-api.com/update";
            string token;

            try
            {
                token = await tokenHandler.GetTokenAsync();
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to retrieve token.");
                return;
            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(updateApiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    log.LogInformation($"Order {order.OrderId} updated successfully.");
                }
                else
                {
                    log.LogError($"Failed to update order: Order {order.OrderId}. Status Code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, $"HTTP request error while updating order: Order {order.OrderId}");
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Unexpected error while updating order: Order {order.OrderId}");
            }
        }
    }
}
