using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TinyCard.Core.Services.Extensions;
using TinyCard.Core.Services.Options;

namespace TinyCard.Core.Tests
{
    public class TinyCardFixture : IDisposable
    {
        public IServiceScope Scope { get; private set; }

        public TinyCardFixture()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}")
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Initialize Dependency container
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddAppServices(config);

            Scope = serviceCollection
                .BuildServiceProvider()
                .CreateScope();
        }

        public void Dispose()
        {
            Scope.Dispose();
        }
    }
}
