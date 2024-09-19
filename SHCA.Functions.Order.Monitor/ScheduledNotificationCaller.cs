using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using AuthenticationResult = Microsoft.Identity.Client.AuthenticationResult;

namespace SHCA.Functions.Order
{
    // Class included as example of monitoring scheduler to run the order notification caller - would function end to end in a production environment.
    public class ScheduledNotificationCaller
    {
        private static readonly HttpClient httpClient = new HttpClient();

        [FunctionName("ProcessOrders")]
        public static async Task Run(
        [TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, // Runs every 5 minutes, arbitrary
        ILogger log)
        {
            log.LogInformation($"Process Orders Timer trigger function executed at: {DateTime.Now}");

            string ordersProcessingApiUrl = "https://monitor-api.com/process-orders"; // In production this value would come from an azure config setup
            string[] scopes = new string[] { "api://scope/.default" };

            // In production this value would come from an azure config setup
            var confidentialClientApp = ConfidentialClientApplicationBuilder.Create(Environment.GetEnvironmentVariable("AzureAd:ClientId"))
                .WithClientSecret(Environment.GetEnvironmentVariable("AzureAd:ClientSecret"))
                .WithAuthority(new Uri($"{Environment.GetEnvironmentVariable("AzureAd:Instance")}{Environment.GetEnvironmentVariable("AzureAd:TenantId")}"))
                .Build();

            AuthenticationResult result = await confidentialClientApp.AcquireTokenForClient(scopes).ExecuteAsync();
            string token = result.AccessToken;

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.PostAsync(ordersProcessingApiUrl, null);
            if (response != null && response.IsSuccessStatusCode)
            {
                log.LogInformation("Orders processed successfully.");
            }
            else
            {
                log.LogError("Failed to process orders from API.");
            }
        }
    }
}

