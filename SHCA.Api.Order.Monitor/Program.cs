
using Microsoft.Identity.Client;
using SHCA.App.OrderProcessing.Monitor.Process;
using SHCA.Domain.Entities;
using SHCA.Domain.Interfaces.Repositories;
using SHCA.Infra.Repositories;

namespace SHCA.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddHttpClient<OrderStatusProcessorService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IConfidentialClientApplication>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                return ConfidentialClientApplicationBuilder.Create(config["AzureAd:ClientId"])
                    .WithClientSecret(config["AzureAd:ClientSecret"])
                    .WithAuthority(new Uri($"{config["AzureAd:Instance"]}{config["AzureAd:TenantId"]}"))
                    .Build();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
