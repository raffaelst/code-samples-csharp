using Dividendos.API.Model.Request.Crypto;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Crypto;
using Dividendos.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dividendos.API.Controllers.v1
{
    [Authorize("Bearer")]
    [Route("[controller]")]
    [ApiController]
    public class CryptoTransactionController : BaseController
    {
        private readonly ICryptoTransactionApp _cryptoTransactionApp;

        public CryptoTransactionController(ICryptoTransactionApp cryptoTransactionApp)
        {
            _cryptoTransactionApp = cryptoTransactionApp;
        }

        /// <summary>
        /// Add Buy Crypto
        /// </summary>
        /// <returns></returns>
        [HttpPost("buycrypto/{guidCryptoPortfolio}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> BuyCrypto(Guid guidCryptoPortfolio, CryptoAddVM cryptoAddVM)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<CryptoBuyVM> resultResponse = _cryptoTransactionApp.BuyCrypto(guidCryptoPortfolio, cryptoAddVM);

            return Response(resultResponse);
        }

        /// <summary>
        /// Add Sell Crypto
        /// </summary>
        /// <returns></returns>
        [HttpPost("sellcrypto/{guidCryptoPortfolio}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SellCrypto(Guid guidCryptoPortfolio, CryptoAddVM cryptoAddVM)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _cryptoTransactionApp.SellCrypto(guidCryptoPortfolio, cryptoAddVM);

            return Response(resultResponse);
        }

        /// <summary>
        /// Edit Buy Transaction
        /// </summary>
        /// <returns></returns>
        [HttpPut("editbuytransaction/{guidCryptoPortfolio}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> EditBuyCrypto(Guid guidCryptoPortfolio, CryptoEditVM cryptoEditVM)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _cryptoTransactionApp.EditBuyTransaction(guidCryptoPortfolio, cryptoEditVM);

            return Response(resultResponse);
        }

        /// <summary>
        /// Edit Sell Transaction
        /// </summary>
        /// <returns></returns>
        [HttpPut("editselltransaction/{guidCryptoPortfolio}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> EditSellCrypto(Guid guidCryptoPortfolio, CryptoEditVM cryptoEditVM)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _cryptoTransactionApp.EditSellTransaction(guidCryptoPortfolio, cryptoEditVM);

            return Response(resultResponse);
        }

        /// <summary>
        /// Inactive Operation
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{guidCryptoPortfolio}/cryptotransactionitem/{guidCryptoTransactionItem}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> InactiveCryptoTransactionItem(Guid guidCryptoPortfolio, Guid guidCryptoTransactionItem)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _cryptoTransactionApp.InactiveCryptoTransactionItem(guidCryptoPortfolio, guidCryptoTransactionItem);

            return Response(resultResponse);
        }

        /// <summary>
        /// Get Transaction Items
        /// </summary>
        /// <returns></returns>
        [HttpGet("{guidCryptoPortfolio}/cryptotransactionitem/{guidCryptoCurrency}/{transactionType}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTransactionItemSummary(Guid guidCryptoPortfolio, Guid guidCryptoCurrency, int transactionType)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _cryptoTransactionApp.GetTransactionItemSummary(guidCryptoPortfolio, guidCryptoCurrency, transactionType);

            return Response(resultResponse);
        }

        /// <summary>
        /// Get Transaction Sell Items
        /// </summary>
        /// <returns></returns>
        [HttpGet("cryptotransactionSellDetails/{guidCryptoPortfolioSub}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCryptoTransactionSellDetails(Guid guidCryptoPortfolioSub, [FromQuery] string startDate, [FromQuery] string endDate)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _cryptoTransactionApp.GetCryptoTransactionSellView(guidCryptoPortfolioSub, startDate, endDate);

            return Response(resultResponse);
        }

        /// <summary>
        /// Get Transaction Buy Items
        /// </summary>
        /// <returns></returns>
        [HttpGet("cryptotransactionBuyDetails/{guidCryptoPortfolioSub}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCryptoTransactionBuyDetails(Guid guidCryptoPortfolioSub, [FromQuery] string startDate, [FromQuery] string endDate)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _cryptoTransactionApp.GetCryptoTransactionBuyView(guidCryptoPortfolioSub, startDate, endDate);

            return Response(resultResponse);
        }
    }
}
