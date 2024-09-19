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
    public class AlertService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger log;
        private readonly ITokenHandler tokenHandler;

        public AlertService(HttpClient httpClient, ILogger logger, ITokenHandler tokenHandler)
        {
            this.httpClient = httpClient;
            log = logger;
            this.tokenHandler = tokenHandler;
        }

        public async Task SendAlertMessage(OrderItem item, string orderId)
        {
            string alertsApiUrl = "https://alert-api.com/alerts";
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

            var alertData = new
            {
                Message = $"Alert for delivered item: Order {orderId}, Item: {item.Description}, Delivery Notifications: {item.Status}"
            };

            var content = new StringContent(JsonConvert.SerializeObject(alertData), Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(alertsApiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    log.LogInformation($"Alert for delivered item: Order {orderId}, Item: {item.Description}, Delivery Notifications: {item.Status}");
                }
                else
                {
                    log.LogError($"Failed to send alert for delivered item: Order {orderId}, Item: {item.Description}, Delivery Notifications: {item.Status}. Response: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, $"HTTP request error while sending alert for delivered item: Order {orderId}, Item: {item.Description}");
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Unexpected error while sending alert for delivered item: Order {orderId}, Item: {item.Description}");
            }
        }
    }
}