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

using Xunit;

using TinyCard.Core.Services.Extensions;

namespace TinyCard.Core.Tests
{
    public class LimitTest : IClassFixture<TinyCardFixture>
    {
        private ILimitService _limit;
        private Data.TinyCardDbContext _dbContext;

        public LimitTest(TinyCardFixture fixture)
        {
            _limit = fixture.Scope.ServiceProvider
                .GetRequiredService<ILimitService>();

            _dbContext = fixture.Scope.ServiceProvider
                .GetRequiredService<Data.TinyCardDbContext>();
        }

        [Fact]
        public async Task<Limit> Add_New_Limit_Success()
        {
            var options = new Services.Options.CardLimitOptions()
            {
                CardNumber = "1111222233334444",
                AvailableBalance = Constants.AmountLimits.CardPresent,
                TrsansactionType = CardTransactionType.CardPresent
            };

            var limit = (await _limit.RegisterCardLimitAsync(options))?.Data;

            Assert.NotNull(limit);

            var savedLimit = _dbContext.Set<Limit>()
                .Where(c => c.ID == limit.ID)
                .SingleOrDefault();
            Assert.NotNull(savedLimit);
            Assert.Equal(options.CardNumber, savedLimit.CardNumber);
            Assert.Equal(options.AvailableBalance, savedLimit.AvailableBalance);
            Assert.Equal(options.TrsansactionType, savedLimit.TrsansactionType);

            return limit;

        }

        [Fact]
        public async Task<Limit> Add_New_Limit2_Success()
        {
            var options = new Services.Options.CardLimitOptions()
            {
                CardNumber = "1111222233334444",
                AvailableBalance = Constants.AmountLimits.ECommerce,
                TrsansactionType = CardTransactionType.ECommerce
            };

            var limit = (await _limit.RegisterCardLimitAsync(options))?.Data;

            Assert.NotNull(limit);

            var savedLimit = _dbContext.Set<Limit>()
                .Where(c => c.ID == limit.ID)
                .SingleOrDefault();
            Assert.NotNull(savedLimit);
            Assert.Equal(options.CardNumber, savedLimit.CardNumber);
            Assert.Equal(options.AvailableBalance, savedLimit.AvailableBalance);
            Assert.Equal(options.TrsansactionType, savedLimit.TrsansactionType);

            return limit;

        }

        [Fact]
        public async Task<Limit> UpdateLimitAsync_Success()
        {
            var options = new Services.Options.CardLimitOptions()
            {
                CardNumber = "2222333344445555",
                AvailableBalance = 100,
                TrsansactionType =  CardTransactionType.CardPresent
            };

            var result = await _limit.UpdateCardLimitAsync(options);
            Assert.Equal(ResultCode.Success, result.Code);
            Assert.Equal(options.CardNumber, result.Data.CardNumber);

            return result.Data;
        }

    }
}
