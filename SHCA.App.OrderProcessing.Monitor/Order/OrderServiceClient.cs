using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SHCA.App.OrderProcessing.Monitor.Auth;
using SHCA.App.OrderProcessing.Monitor.Interfaces;

namespace SHCA.App.OrderProcessing.Monitor.Process
{
    public class OrderServiceClient : IOrderServiceClient
    {
        private readonly HttpClient httpClient;
        private readonly ILogger log;
        private readonly ITokenHandler tokenHandler;

        public OrderServiceClient(HttpClient httpClient, ILogger<OrderServiceClient> logger, ITokenHandler tokenHandler)
        {
            this.httpClient = httpClient;
            this.log = logger;
            this.tokenHandler = tokenHandler;
        }

        public async Task<string?> FetchMedicalEquipmentOrders()
        {
            string ordersApiUrl = "https://orders-api.com/orders";
            string token = await tokenHandler.GetTokenAsync();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync(ordersApiUrl);

            if (response.IsSuccessStatusCode)
            {
                log.LogInformation("Orders fetched successfully.");
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                log.LogError("Failed to fetch orders from API.");
                return null;
            }
        }
    }
}
