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
            billingClient.Invoices.List().ToList().ForEach(inv => {
                Write("\tName: {0}, Id: {1}", inv.Name, inv.Id);
            });
            Write(Environment.NewLine);

            //TODO: handle 404
        }
        public static async Task<BillingClient> GetBillingClient()
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            var tenantId = Configuration["TenantDomain"];
            var clientId = Configuration["ClientID"];
            var secret = Configuration["ClientSecret"];
            var subscriptionId = Configuration["SubscriptionId"];

            if(new List<string>{ tenantId, clientId, secret, subscriptionId }.Any(i => String.IsNullOrEmpty(i))) {
                throw new InvalidOperationException("Please provide TenantDomain, ClientID, ClientSecret and SubscriptionId in appsettings.json");
            }
            else
            {
                var serviceCreds = await ApplicationTokenProvider.LoginSilentAsync(tenantId, clientId, secret);
                var billingClient = new BillingClient(serviceCreds);
                billingClient.SubscriptionId = subscriptionId;
                return billingClient;
            }
        }        
    }
}
