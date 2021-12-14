using Azure.Identity;
using Back_Market_Vinci.DataServices;
using Back_Market_Vinci.Domaine;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci
{
    public class Program
    {

        public static void Main(string[] args)
        {
             CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                })
            .ConfigureAppConfiguration((context, config) =>
            {
                var builtConfig = config.Build();
                var vaultUrl = builtConfig["VaultUrl"];
                var keyVaultClient = new KeyVaultClient(async (authority, resource, scope) =>
                {
                    var credential = new DefaultAzureCredential(false);
                    var token = credential.GetToken(
                        new Azure.Core.TokenRequestContext(
                            new[] { "https://vault.azure.net/.default" }
                            )
                        );
                    return token.Token;
                });
                config.AddAzureKeyVault(vaultUrl, keyVaultClient, new DefaultKeyVaultSecretManager());
            });
    }
}
