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

namespace Dividendos.API.Controllers.v1
{
    [Authorize("Bearer")]
    [Route("[controller]")]
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
        public async Task<IActionResult> GetByTrader([FromQuery]Guid? traderGuid)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<CryptoPortfolioStatementWrapperVM> resultServiceObject = _cryptoCurrencyApp.GetCryptosByTrader(traderGuid);

            return Response(resultServiceObject);
        }


        [HttpGet("brokers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetListOfBrokers()
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<IEnumerable<CryptoBrokerVM>> resultServiceObject = _cryptoCurrencyApp.GetCryptosBroker();

            return Response(resultServiceObject);
        }

        /// <summary>
        /// Get cryptos by name ou symbol (Auto complete)
        /// </summary>
        /// <returns></returns>
        [HttpGet("autocomplete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetBySymbol(string symbolOrName)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

             ResultResponseObject<IEnumerable<CryptoInfoVM>> resultResponse = _cryptoCurrencyApp.GetCryptosInfo(symbolOrName);

            return Response(resultResponse);
        }

        /// <summary>
        /// Update average price 
        /// </summary>
        /// <param name="financialProductAddVM"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateAveragePrice([FromBody] FinancialProductAddVM financialProductAddVM)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<FinancialProductAddVM> resultServiceObject = _cryptoCurrencyApp.UpdateAveragePrice(financialProductAddVM);

            return Response(resultServiceObject);
        }
    }
}
