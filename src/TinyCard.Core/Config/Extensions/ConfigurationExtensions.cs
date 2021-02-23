﻿using Microsoft.Extensions.Configuration;

namespace TinyCard.Core.Config.Extensions
{
    public static class ConfigurationExtensions
    {
        public static AppConfig ReadAppConfiguration(
            this IConfiguration @this)
        {
            var minLoggingLevel = @this.GetSection("MinLoggingLevel").Value;
            var connectionString = @this.GetConnectionString("TinyCardDatabase");

            var clientId = @this.GetSection("ClientConfig")
                .GetSection("clientId").Value;

            var clientSecret = @this.GetSection("ClientConfig")
                .GetSection("clientSecret").Value;

            return new AppConfig()
            {
                TinyCardConnectionString = connectionString,
                MinLoggingLevel = minLoggingLevel,
                ClientConfig = new ClientConfig()
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                }
            };
        }
    }
}
