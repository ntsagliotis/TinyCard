using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using TinyCard.Core.Data;
using TinyCard.Core.Config.Extensions;

namespace TinyCard.ConsoleApp
{
    public class DbContextFactory : IDesignTimeDbContextFactory<TinyCardDbContext>
    {
        public TinyCardDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}")
                .AddJsonFile("appsettings.json", false)
                .Build();

            var config = configuration.ReadAppConfiguration();

            var optionsBuilder = new DbContextOptionsBuilder<TinyCardDbContext>();

            optionsBuilder.UseSqlServer(
                config.TinyCardConnectionString,
                options => {
                    options.MigrationsAssembly("TinyCard.ConsoleApp");
                });

            return new TinyCardDbContext(optionsBuilder.Options);
        }
    }
}
