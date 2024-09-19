using System;
using System.Net.Http;
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
            string token;

            try
            {
                token = await tokenHandler.GetTokenAsync();
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to retrieve token.");
                return null;
            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await httpClient.GetAsync(ordersApiUrl);

                if (response.IsSuccessStatusCode)
                {
                    log.LogInformation("Orders fetched successfully.");
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    log.LogError($"Failed to fetch orders from API. Status Code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, "HTTP request error while fetching orders.");
                return null;
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Unexpected error while fetching orders.");
                return null;
            }
        }
    }
}
