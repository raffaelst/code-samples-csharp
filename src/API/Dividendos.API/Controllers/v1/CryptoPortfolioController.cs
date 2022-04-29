using Dividendos.API.Model.Request.Crypto;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Crypto;
using Dividendos.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.API.Controllers.v1
{
    [Authorize("Bearer")]
    [Route("[controller]")]
    [ApiController]
    public class CryptoPortfolioController : BaseController
    {
        private readonly ICryptoPortfolioApp _cryptoPortfolioApp;
        private readonly ICryptoSubPortfolioApp _cryptoSubPortfolioApp;

        public CryptoPortfolioController(ICryptoPortfolioApp cryptoPortfolioApp, ICryptoSubPortfolioApp cryptoSubPortfolioApp)
        {
            _cryptoPortfolioApp = cryptoPortfolioApp;
            _cryptoSubPortfolioApp = cryptoSubPortfolioApp;
        }

        /// <summary>
        /// Create a new Manual Portfolio
        /// </summary>
        /// <returns></returns>
        [HttpPost("manualportfolio")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateManualPortfolio(CryptoPortolioRequest cryptoPortolioRequest)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<CryptoPortfolioVM> resultResponse = _cryptoPortfolioApp.CreateManualPortfolio(cryptoPortolioRequest);

            return Response(resultResponse);
        }

        /// Get Crypto Portfolio View
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("cryptoportfolioview")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCryptoPortfolioView()
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<CryptoPortfolioViewWrapperVM> resultServiceObject = _cryptoPortfolioApp.GetCryptoPortfolioViewWrapper();

            return Response(resultServiceObject);
        }

        /// <summary>
        /// Calculate Crypto Portfolio Performance by User
        /// </summary>
        /// <returns></returns>
        [HttpPut("calculateperformance")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CalculatePerformance()
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _cryptoPortfolioApp.CalculatePerformance();

            return Response(resultResponse);
        }

        /// Get Crypto Portfolio Statement
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("cryptoportfoliostatement/{guidCryptoPortfolioSub}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCryptoPortfolioStatementView(Guid guidCryptoPortfolioSub)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<CryptoCurrencyStatementWrapperVM> resultServiceObject = _cryptoPortfolioApp.GetCryptoCurrencyStatementWrapperVM(guidCryptoPortfolioSub);

            return Response(resultServiceObject);
        }

        /// <summary>
        /// Get currency statement by guidCryptoCurrency
        /// </summary>
        /// <returns></returns>
        [HttpGet("{guidCryptoPortfolio}/cryptoCurrencystatement/{guidCryptoCurrency}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCryptoCurrencyStatementView(Guid guidCryptoPortfolio, Guid guidCryptoCurrency)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<CryptoCurrencyStatementVM> resultResponse = _cryptoPortfolioApp.GetCryptoCurrencyStatementView(guidCryptoPortfolio, guidCryptoCurrency);

            return Response(resultResponse);
        }

        /// <summary>
        /// Delete a crypto portfolio
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{guidCryptoPortfolio}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete(Guid guidCryptoPortfolio)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponseBase = _cryptoPortfolioApp.Disable(guidCryptoPortfolio);

            return Response(resultResponseBase);
        }

        /// <summary>
        /// Create a new Crypto SubPortfolio
        /// </summary>
        /// <returns></returns>
        [HttpPost("{guidCryptoPortfolio}/cryptosubportfolio")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostSubPortfolio(Guid guidCryptoPortfolio, CryptoSubportfolioVM cryptoSubportfolioVM)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<CryptoSubportfolioVM> resultResponse = _cryptoSubPortfolioApp.Add(guidCryptoPortfolio, cryptoSubportfolioVM);

            return Response(resultResponse);
        }

        /// <summary>
        /// Delete a SubPortfolio
        /// </summary>
        /// <returns></returns>
        [HttpDelete("cryptosubportfolio/{guidCryptoSubPortfolio}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteCryptoSubPortfolio(Guid guidCryptoSubPortfolio)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponseBase = _cryptoSubPortfolioApp.Disable(guidCryptoSubPortfolio);

            return Response(resultResponseBase);
        }

        /// <summary>
        /// Update Crypto Portfolio Name
        /// </summary>
        /// <returns></returns>
        [HttpPut("{guidCryptoPortfolio}/updatename")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(Guid guidCryptoPortfolio, CryptoPortfolioEditVM cryptoPortfolioEditVM)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _cryptoPortfolioApp.UpdateName(guidCryptoPortfolio, cryptoPortfolioEditVM);

            return Response(resultResponse);
        }

        /// <summary>
        /// Get details of specific crypto portfolio
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("{guidCryptoPortfolio}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCryptoPortfolioDetail(Guid guidCryptoPortfolio)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<CryptoSubportfolioWrapperVM> resultServiceObject = _cryptoPortfolioApp.GetCryptoPortfolioContentSimple(guidCryptoPortfolio);

            return Response(resultServiceObject);
        }

        /// <summary>
        /// Get details of specific subportfolio
        /// </summary>
        /// <returns></returns>
        [HttpGet("{guidCryptoPortfolio}/cryptosubportfolio/{guidCryptoSubportfolio}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCryptoSubportfolioDetails(Guid guidCryptoPortfolio, Guid guidCryptoSubportfolio)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<CryptoSubportfolioWrapperVM> resultServiceObject = _cryptoPortfolioApp.GetCryptoSubportfolioContentSimple(guidCryptoPortfolio, guidCryptoSubportfolio);

            return Response(resultServiceObject);
        }

        /// <summary>
        /// Edit a CryptoSubportfolio
        /// </summary>
        /// <returns></returns>
        [HttpPut("{guidCryptoSubportfolio}/subportfolio")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutSubPortfolio(Guid guidCryptoSubportfolio, CryptoSubportfolioVM cryptoSubportfolioVM)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<CryptoSubportfolioVM> resultResponse = _cryptoSubPortfolioApp.Update(guidCryptoSubportfolio, cryptoSubportfolioVM);

            return Response(resultResponse);
        }
    }
}
