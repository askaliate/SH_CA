using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

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
                try
                {
                    // In production this value would come from an azure config setup

                    var confidentialClientApp = ConfidentialClientApplicationBuilder.Create(Environment.GetEnvironmentVariable("AzureAd:ClientId"))
                        .WithClientSecret(Environment.GetEnvironmentVariable("AzureAd:ClientSecret"))
                        .WithAuthority(new Uri($"{Environment.GetEnvironmentVariable("AzureAd:Instance")}{Environment.GetEnvironmentVariable("AzureAd:TenantId")}"))
                        .Build();

                    AuthenticationResult result = await confidentialClientApp.AcquireTokenForClient(scopes).ExecuteAsync();
                    token = result.AccessToken;
                }
                catch (MsalServiceException ex)
                {
                    // Handle MSAL service exceptions (e.g., AADSTS errors)
                    throw new Exception("MSAL service exception occurred while acquiring token.", ex);
                }
                catch (MsalClientException ex)
                {
                    // Handle MSAL client exceptions (e.g., configuration errors)
                    throw new Exception("MSAL client exception occurred while acquiring token.", ex);
                }
                catch (Exception ex)
                {
                    // Handle general exceptions
                    throw new Exception("An unexpected error occurred while acquiring token.", ex);
                }
            }

            return token;
        }
    }
}