using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;

using TinyCard.Core.Data;
using TinyCard.Core.Model;
using TinyCard.Core.Services;
using TinyCard.Core.Constants;
using TinyCard.Core.Services.Options;
using TinyCard.Core.Config.Extensions;
using Microsoft.Extensions.DependencyInjection;
using TinyCard.Core.Constants;

using Xunit;

namespace TinyCard.Core.Tests
{
    public class CardTest : IClassFixture<TinyCardFixture>
    {
        private ICardService _card;
        private Data.TinyCardDbContext _dbContext;

        public CardTest(TinyCardFixture fixture)
        {
            _card = fixture.Scope.ServiceProvider
                .GetRequiredService<ICardService>();

            _dbContext = fixture.Scope.ServiceProvider
                .GetRequiredService<Data.TinyCardDbContext>();
        }

        [Fact]
        public async Task<Card> Add_New_Card_Success()
        {
            var options = new Services.Options.CardOptions()
            {
                CardNumber = "1111222233334444",
                CardHolderName = "Name1",
                CardHolderSurname = "Surname1"
            };
            
            var card = (await _card.RegisterCardAsync(options))?.Data;

            Assert.NotNull(card);

            var savedCard = _dbContext.Set<Card>()
                .Where(c => c.ID == card.ID)
                .SingleOrDefault();
            Assert.NotNull(savedCard);
            Assert.Equal(options.CardNumber, savedCard.CardNumber);
            Assert.Equal(options.CardHolderName, savedCard.CardHolderName);
            Assert.Equal(options.CardHolderSurname, savedCard.CardHolderSurname);

            return card;
            
        }

        [Fact]
        public async Task<Card> Add_New_Card2_Success()
        {
            var options = new Services.Options.CardOptions()
            {
                CardNumber = "2222333344445555",
                CardHolderName = "Name2",
                CardHolderSurname = "Surname2"
            };

            var card = (await _card.RegisterCardAsync(options))?.Data;

            Assert.NotNull(card);

            var savedCard = _dbContext.Set<Card>()
                .Where(c => c.ID == card.ID)
                .SingleOrDefault();
            Assert.NotNull(savedCard);
            Assert.Equal(options.CardNumber, savedCard.CardNumber);
            Assert.Equal(options.CardHolderName, savedCard.CardHolderName);
            Assert.Equal(options.CardHolderSurname, savedCard.CardHolderSurname);

            return card;

        }
        private DbContextOptionsBuilder<TinyCardDbContext> GetDBOptions()
        {
            var config = new ConfigurationBuilder()
               .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}")
               .AddJsonFile("appsettings.json", false)
               .Build();


            var connString = config.ReadAppConfiguration();

            var options = new DbContextOptionsBuilder<TinyCardDbContext>();
            options.UseSqlServer(connString.TinyCardConnectionString,
                options =>
                {
                    options.MigrationsAssembly("TinyCard");
                });

            return options;
        }



    }
}
