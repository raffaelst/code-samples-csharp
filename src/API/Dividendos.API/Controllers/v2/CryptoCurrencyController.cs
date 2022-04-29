using Dividendos.API.Model.Request;
using Dividendos.API.Model.Request.Crypto;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Crypto;
using Dividendos.API.Model.Response.v2;
using Dividendos.Application.Interface;
using Dividendos.Application.Interface.Model;
using Dividendos.CrossCutting.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.API.Controllers.v2
{
    [Authorize("Bearer")]
    [ApiVersion("2")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CryptoCurrencyController : BaseController
    {
        private readonly ICryptoCurrencyApp _cryptoCurrencyApp;

        public CryptoCurrencyController(ICryptoCurrencyApp cryptoCurrencyApp)
        {
            _cryptoCurrencyApp = cryptoCurrencyApp;
        }


        /// <summary>
        /// Get list by trader (used in extract screen)
        /// </summary>
        /// <param name="traderGuid"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetByTrader([FromQuery] Guid? traderGuid)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<CryptoPortfolioStatementWrapperVM> resultServiceObject = _cryptoCurrencyApp.GetCryptosByTrader(traderGuid);

            return Response(resultServiceObject);
        }

        [HttpGet("market-movers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> MarketMovers([FromQuery] CryptoMakertMoversType cryptoMakertMoversType)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<IEnumerable<API.Model.Response.CryptoMarketMoverVM>> resultResponse = _cryptoCurrencyApp.GetMarketMoverByType(cryptoMakertMoversType);

            return Response(resultResponse);
        }
    }
}
