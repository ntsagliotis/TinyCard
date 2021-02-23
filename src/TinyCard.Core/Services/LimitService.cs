using System.Threading.Tasks;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using TinyCard.Core.Constants;
using TinyCard.Core.Data;
using TinyCard.Core.Model;
using TinyCard.Core.Results;
using TinyCard.Core.Services.Options;
using System.Globalization;

namespace TinyCard.Core.Services
{
    public class LimitService : ILimitService
    {
        private TinyCardDbContext _dbContext;
        private ICardService _cardService;

        public LimitService(TinyCardDbContext dbContext, ICardService card)
        {
            _dbContext = dbContext;
            _cardService = card;
        }

        public async Task<Result<Limit>> GetBalanceByCardNumber(string CardNumbre)
        {
            var options = new Services.Options.CardLimitOptions()
            {
                CardNumber = CardNumbre,
                AvailableBalance = 0
            };

            var result = await _cardService.CardExists(CardNumbre);

            if (result.Data == false)
            {
                return new Result<Limit>()
                {
                    Code = result.Code,
                    Message = result.Message
                };
            }

            var limit = _dbContext.Set<Limit>()
                .Where(l => l.CardNumber == options.CardNumber)
                .Where(l => l.TrsansactionType == CardTransactionType.ECommerce)
                .SingleOrDefault();

            var limit2 = _dbContext.Set<Limit>()
                .Where(l => l.CardNumber == options.CardNumber)
                .Where(l => l.TrsansactionType == CardTransactionType.CardPresent)
                .SingleOrDefault();

            limit.AvailableBalance = limit.AvailableBalance + limit2.AvailableBalance;

            return new Result<Limit>()
            {
                Code = ResultCode.Success,
                Message = "Balance found",
                Data = limit
            };
        }

        public async Task<Result<Limit>> GetCardLimitAsync(CardLimitOptions options)
        {
            var result = await _cardService.CardExists(options.CardNumber);

            // Card not found
            if (result.Data == false)
            {
                return new Result<Limit>()
                {
                    Code = result.Code,
                    Message = result.Message
                };

            }

            var limit = _dbContext.Set<Limit>()
                .Where(l => l.CardNumber == options.CardNumber)
                .Where(l => l.TrsansactionType == options.TrsansactionType)
                .SingleOrDefault();

            if (limit != null)
            {
                // limit found Check Date.
                if (limit.ReferenceDate.Date != DateTime.Now.Date)
                {
                    // if older date assign to today and update
                    options.ReferenceDate = DateTime.Now.Date;

                    if (options.TrsansactionType == CardTransactionType.CardPresent)
                        options.AvailableBalance = Constants.AmountLimits.CardPresent - options.AvailableBalance;
                    else if (options.TrsansactionType == CardTransactionType.ECommerce)
                        options.AvailableBalance = Constants.AmountLimits.ECommerce - options.AvailableBalance;

                    var result2 = await UpdateCardLimitAsync(options);
                }
                return new Result<Limit>()
                {
                    Code = ResultCode.Success,
                    Data = limit
                };
            }
            else
            {
                // Register new limit
                var register = await RegisterCardLimitAsync(options);
                if (register.Code == ResultCode.Success)
                {
                    return new Result<Limit>()
                    {
                        Code = ResultCode.Success,
                        Data = limit
                    };
                }
                else
                {
                    return new Result<Limit>()
                    {
                        Code = ResultCode.InternalServerError,
                        Message = "Failed to create limit",
                        Data = limit
                    };
                }
            }
        }

        public async Task<Result<Limit>> RegisterCardLimitAsync(CardLimitOptions options)
        {
            // Validations done earlier
            decimal Balance = 0;
            if (options.TrsansactionType == CardTransactionType.CardPresent)
                Balance = Constants.AmountLimits.CardPresent;
            else if (options.TrsansactionType == CardTransactionType.ECommerce)
                Balance = Constants.AmountLimits.ECommerce;

            var limit = new Limit()
            {
                CardNumber = options.CardNumber,
                AvailableBalance = Balance,
                TrsansactionType = options.TrsansactionType,
                ReferenceDate = DateTime.Now.Date
            };

            _dbContext.Add(limit);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new Result<Limit>()
                {
                    Code = Constants.ResultCode.InternalServerError,
                    Message = "Card limit could not be saved"
                };
            }

            return new Result<Limit>()
            {
                Code = Constants.ResultCode.Success,
                Data = limit
            };
        }

        public async Task<Result<Limit>> UpdateCardLimitAsync(CardLimitOptions options)
        {
            decimal Balance = 0;
            var result = await _cardService.CardExists(options.CardNumber);

            // Card not found
            if (result.Data == false)
            {
                return new Result<Limit>()
                {
                    Code = result.Code,
                    Message = result.Message
                };

            }

            var limit = _dbContext.Set<Limit>()
                .Where(l => l.CardNumber == options.CardNumber)
                .Where(l => l.TrsansactionType == options.TrsansactionType)
                .Where(l => l.ReferenceDate.Date ==  DateTime.Now.Date )
                .SingleOrDefault();

            if (limit != null)
            {
                var newlimit = new Limit()
                {
                    CardNumber = options.CardNumber,
                    AvailableBalance = limit.AvailableBalance - options.AvailableBalance,
                    TrsansactionType = options.TrsansactionType,
                    ReferenceDate = DateTime.Now.Date
                };

                _dbContext.Update(limit);
                return new Result<Limit>()
                {
                    Code = ResultCode.Success,
                    Data = newlimit
                };
            }
            else
            {
                // Register new limit
                var register = await RegisterCardLimitAsync(options);
                if (register.Code == ResultCode.Success)
                {
                    return new Result<Limit>()
                    {
                        Code = ResultCode.Success,
                        Data = limit
                    };
                }
                else
                {
                    return new Result<Limit>()
                    {
                        Code = ResultCode.InternalServerError,
                        Message = "Failed to create limit",
                        Data = limit
                    };
                }
            }
        }



    }
}
