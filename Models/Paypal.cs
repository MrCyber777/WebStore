using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebStore.Models
{
    public class Paypal
    {
        public static PayPalConfig GetPayPalConfig()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            return new PayPalConfig()
            {
                AuthToken = configuration["PayPal:AuthToken"],
                Business = configuration["PayPal:Business"],
                PostUrl = configuration["PayPal:PostUrl"]
            };
        }
    }
}