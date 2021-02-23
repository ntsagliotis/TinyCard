using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TinyCard.Core.Data;
using TinyCard.Core.Config;
using TinyCard.Core.Config.Extensions;

namespace TinyCard.Core.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppServices(
            this IServiceCollection @this, IConfiguration configuration)
        {
            @this.AddSingleton<AppConfig>(
                configuration.ReadAppConfiguration());

            // AddScoped
            @this.AddDbContext<TinyCardDbContext>(
                (serviceProvider, optionsBuilder) => {
                    var appConfig = serviceProvider.GetRequiredService<AppConfig>();

                    optionsBuilder.UseSqlServer(appConfig.TinyCardConnectionString);
                });

            @this.AddScoped<ICardService, CardService>();
            @this.AddScoped<ILimitService, LimitService>();
        }
    }
}
