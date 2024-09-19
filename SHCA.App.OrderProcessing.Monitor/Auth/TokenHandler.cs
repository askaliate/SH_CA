using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCA.App.OrderProcessing.Monitor.Auth
{
    public class TokenHandler : ITokenHandler
    {
        private readonly string[] scopes = new string[] { "api://scope/.default" };
        private string? token;

        public async Task<string> GetTokenAsync()
        {

            if (string.IsNullOrEmpty(token))
            {
                // In production this value would come from an azure config setup
                var confidentialClientApp = ConfidentialClientApplicationBuilder.Create(Environment.GetEnvironmentVariable("AzureAd:ClientId"))
                    .WithClientSecret(Environment.GetEnvironmentVariable("AzureAd:ClientSecret"))
                    .WithAuthority(new Uri($"{Environment.GetEnvironmentVariable("AzureAd:Instance")}{Environment.GetEnvironmentVariable("AzureAd:TenantId")}"))
                    .Build();

                AuthenticationResult result = await confidentialClientApp.AcquireTokenForClient(scopes).ExecuteAsync();
                token = result.AccessToken;
            }

            return token;
        }
    }
}
