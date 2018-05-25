using System;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CompanyX.Api
{
    /// <summary>
    /// Application entry
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Do not rename or remove this function 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var inAzure = RunningInAzure();

            // Must set base-path or it will not find the json files
            var configBuilder = new ConfigurationBuilder()
                                    .SetBasePath(Environment.CurrentDirectory);

            if (!inAzure)
            {
                configBuilder = configBuilder.AddJsonFile("appsettings.json", false, true)
                               .AddJsonFile($"appsettings.{environment}.json", true, true);
            }

            var config = configBuilder.AddEnvironmentVariables().Build();

            var builder = WebHost.CreateDefaultBuilder(args);
            if (inAzure)
                builder.UseAzureAppServices();
            else
                builder = builder
                    .UseKestrel(
                        options =>
                        {
                            options.AddServerHeader = false;
                            options.Listen(IPAddress.Loopback, 44354);
                        }
                    );

            return builder
                    .UseConfiguration(config)
                    .UseStartup<Startup>()
                    .Build();
        }

        /// <summary>
        /// How to check if we are in Azure? We simply check for the existence of a certain environment variable. 
        /// See https://shellmonger.com/2017/02/16/running-asp-net-core-applications-in-azure-app-service/.
        /// </summary>
        /// <returns></returns>
        private static bool RunningInAzure()
        {
            return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
        }
    }
}
