using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Extensions.Configuration;

// Azure Management dependencies
using Microsoft.Rest.Azure.Authentication;
using Microsoft.Azure.Management.Billing;
using Microsoft.Azure.Management.Billing.Models;

namespace billing_dotnet_invoice_api
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        private static void Write(string format, params object[] items) 
        {
            Console.WriteLine(String.Format(format, items));
        }
        public static void Main(string[] args)
        {
            Run().Wait();
        }
        public static async Task Run()
        {
            BillingClient billingClient = await GetBillingClient();
            
            Write("Listing invoices:");
            billingClient.Invoices.List("downloadUrl").ToList().ForEach(inv => {
                Write("\tName: {0}, URL: {1}", inv.Name, inv.DownloadUrl.Url);
            });
            Write(Environment.NewLine);

            //TODO: handle 404 invoice not found
        }
        public static async Task<BillingClient> GetBillingClient()
        {
            // Import config values from appsettings.json into billingClient, or throw an error if not found
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            var tenantId = Configuration["TenantDomain"];
            var clientId = Configuration["ClientID"];
            var secret = Configuration["ClientSecret"];
            var subscriptionId = Configuration["SubscriptionID"];

            if(new List<string>{ tenantId, clientId, secret, subscriptionId }.Any(i => String.IsNullOrEmpty(i))) {
                throw new InvalidOperationException("Enter TenantDomain, ClientID, ClientSecret and SubscriptionId in appsettings.json");
            }
            else
            {
                // Build the service credentials and ARM client to call the billing API
                var serviceCreds = await ApplicationTokenProvider.LoginSilentAsync(tenantId, clientId, secret);
                var billingClient = new BillingClient(serviceCreds);
                billingClient.SubscriptionId = subscriptionId;
                return billingClient;
            }
        }        
    }
}
