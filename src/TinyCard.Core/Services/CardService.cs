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
    public class CardService : ICardService
    {
        private TinyCardDbContext _dbContext;

        public CardService(TinyCardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Card>> RegisterCardAsync(CardOptions options)
        {
            if (CardExists(options.CardNumber).Result.Data == true )
            {
                return new Result<Card>()
                {
                    Message = "Card number already exists",
                    Code = Constants.ResultCode.BadRequest
                };
            }

            if (string.IsNullOrWhiteSpace(options?.CardNumber))
            {
                return new Result<Card>()
                {
                    Message = "Card number is empty",
                    Code = Constants.ResultCode.BadRequest
                };
            }

            if (options.CardNumber.Length != 16)
            {
                return new Result<Card>()
                {
                    Message = "Card number requires 16 digits",
                    Code = Constants.ResultCode.BadRequest
                };
            }

            if (string.IsNullOrWhiteSpace(options.CardHolderName) ||
              string.IsNullOrWhiteSpace(options.CardHolderSurname))
            {
                return new Result<Card>()
                {
                    Message = "Name & Surname should not be empty",
                    Code = Constants.ResultCode.BadRequest
                };
            }

            var customer = new Card()
            {
                CardNumber = options.CardNumber,
                CardHolderName = options.CardHolderName,
                CardHolderSurname = options.CardHolderSurname
            };

            _dbContext.Add(customer);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new Result<Card>()
                {
                    Code = Constants.ResultCode.InternalServerError,
                    Message = "Card could not be saved"
                };
            }

            return new Result<Card>()
            {
                Code = Constants.ResultCode.Success,
                Data = customer
            };
        }

        public async Task<Result<Card>> GetCardByNumberAsync(string CardNumber)
        {
            var card = await _dbContext.Set<Card>()
                .Where(c => c.CardNumber == CardNumber)
                .SingleOrDefaultAsync();

            if (card != null)
            {
                return new Result<Card>()
                {
                    Code = ResultCode.Success,
                    Message = $"Card found",
                    Data = card
                };
            }
            else
            {
                return new Result<Card>()
                {
                    Code = ResultCode.NotFound,
                    Message = $"Customer ID {CardNumber} not found !"
                };
            }
        }

        public async Task<Result<bool>> CardExists(string CardNumber)
        {
            var result = await GetCardByNumberAsync(CardNumber);

            if (result.Code == ResultCode.Success)
            {
                return new Result<bool>()
                {
                    Code = result.Code,
                    Message = $"Card ID {CardNumber} found",
                    Data = true
                };
            }
            else
            {
                return new Result<bool>()
                {
                    Code = result.Code,
                    Message = result.Message,
                    Data = false
                };
            }
        }
    }
}
