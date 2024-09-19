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
            string token = await tokenHandler.GetTokenAsync();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var alertData = new
            {
                Message = $"Alert for delivered item: Order {orderId}, Item: {item.Description}, Delivery Notifications: {item.Status}"
            };

            var content = new StringContent(JsonConvert.SerializeObject(alertData), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(alertsApiUrl, content);

            if (response != null && response.IsSuccessStatusCode)
            {
                log.LogInformation($"Alert for delivered item: Order {orderId}, Item: {item.Description}, Delivery Notifications: {item.Status}");
            }
            else
            {
                log.LogError($"Failed to send alert for delivered item: Order {orderId}, Item: {item.Description}, Delivery Notifications: {item.Status}");
            }
        }
    }
}
