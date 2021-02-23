using System.Collections.Generic;
using System.Threading.Tasks;

using TinyCard.Core.Model;
using TinyCard.Core.Results;
using TinyCard.Core.Services.Options;

namespace TinyCard.Core.Services
{
    public interface ILimitService
    {
        public Task<Result<Limit>> GetBalanceByCardNumber(string CardNumber);
        public Task<Result<Limit>> GetCardLimitAsync(CardLimitOptions options);
        public Task<Result<Limit>> RegisterCardLimitAsync(CardLimitOptions options);
        public Task<Result<Limit>> UpdateCardLimitAsync(CardLimitOptions options);
    }
}
