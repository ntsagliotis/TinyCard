using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TinyCard.Core.Constants;
using TinyCard.Core.Model;
using TinyCard.Core.Services;
using TinyCard.Core.Services.Options;

namespace TinyCard.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardLimitController : Controller
    {
        private readonly ILimitService _limit;
        
        private readonly ILogger<CardLimitController> _logger;

        public CardLimitController(
            ILogger<CardLimitController> logger,
            ILimitService limit)
        {
            _logger = logger;
            _limit = limit;
        }

        //[HttpGet]
        //public IActionResult Home()
        //{
        //    return Json("Hello !!!");
        //}

        [HttpGet("{CardNumber}")]
        public async Task<IActionResult> GetByCardNumber(string CardNumber)
        {
            var limit = await _limit.GetBalanceByCardNumber(CardNumber);

            return Json(limit);
        }

        [HttpPost("Authorize")]
        public async Task<IActionResult> Register(
            [FromBody] CardLimitOptions options)
        {
            var result = await _limit.UpdateCardLimitAsync(options);

            if (result.Code != ResultCode.Success)
            {
                return StatusCode(result.Code, result.Message);
            }

            return Json(result);
        }

    }
}
