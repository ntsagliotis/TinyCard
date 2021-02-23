using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyCard.Core.Model;
using TinyCard.Core.Services.Options;
using TinyCard.Core.Results;

namespace TinyCard.Core.Services
{
    public interface ICardService
    {
        public Task<Result<Card>> RegisterCardAsync(CardOptions options);
        public Task<Result<Card>> GetCardByNumberAsync(string CardNumber);
        public Task<Result<bool>> CardExists(string CardNumber);
    }
}
