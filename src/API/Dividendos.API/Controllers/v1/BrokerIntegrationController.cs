using Dividendos.API.Model.Request;
using Dividendos.API.Model.Request.BrokerIntegration;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.BrokerIntegration;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.v1.PortalInvestidorB3;
using Dividendos.Application.Interface;
using Dividendos.Application.Interface.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dividendos.API.Controllers.v1
{
    [Authorize("Bearer")]
    [Route("[controller]")]
    [ApiController]
    public class BrokerIntegrationController : BaseController
    {
        private readonly IBrokerIntegrationApp _brokerIntegrationApp;
        private readonly IPortfolioApp _portfolioApp;

        public BrokerIntegrationController(IBrokerIntegrationApp brokerIntegrationApp,
            IPortfolioApp portfolioApp)
        {
            _brokerIntegrationApp = brokerIntegrationApp;
            _portfolioApp = portfolioApp;
        }

        /// <summary>
        /// Import Mecado BITCOIN
        /// </summary>
        /// <returns></returns>
        [HttpPost("mercadobitcoin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncMercadoBitcoin([FromBody] PortfolioAddVM portfolioAdd)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = _portfolioApp.ImportMercadoBitcoin(portfolioAdd.Document, portfolioAdd.Password);

            return Response(resultResponse);
        }

        /// <summary>
        /// Import Passfolio
        /// </summary>
        /// <returns></returns>
        [HttpPost("passfolio")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncPassfolio([FromBody] PassfolioAddRequest passfolioAddRequest)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<PassfolioAddResponse> resultResponse = _brokerIntegrationApp.AuthenticateOnPassfolio(passfolioAddRequest);

            return Response(resultResponse);
        }

        /// <summary>
        /// Validate Passfolio (2FA)
        /// </summary>
        /// <returns></returns>
        [HttpPut("passfolio-2fa")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncPassfolio2FA([FromBody] Passfolio2FARequest passfolioRequest)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = _portfolioApp.Validate2FAAndImportFromPassfolio(passfolioRequest);

            return Response(resultResponse);
        }

        /// <summary>
        /// Import Binance
        /// </summary>
        /// <returns></returns>
        [HttpPost("binance")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncBinance([FromBody] PortfolioAddVM portfolioAdd)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = _portfolioApp.ImportBinance(portfolioAdd.Document, portfolioAdd.Password);

            return Response(resultResponse);
        }

        /// <summary>
        /// Import BitcoinTrade
        /// </summary>
        /// <returns></returns>
        [HttpPost("bitcointrade")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncBitcoinTrade([FromBody] PortfolioAddVM portfolioAdd)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = _portfolioApp.ImportBitcoinTrade(portfolioAdd.Password);

            return Response(resultResponse);
        }

        /// <summary>
        /// Import Coinbase
        /// </summary>
        /// <returns></returns>
        [HttpPost("coinbase")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncCoinbase([FromBody] PortfolioAddVM portfolioAdd)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = _portfolioApp.ImportCoinbase(portfolioAdd.Document, portfolioAdd.Password);

            return Response(resultResponse);
        }

        /// <summary>
        /// Import BitcoinToYou
        /// </summary>
        /// <returns></returns>
        [HttpPost("bitcointoyou")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncBitcoinToYou([FromBody] PortfolioAddVM portfolioAdd)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = _portfolioApp.ImportBitcoinToYou(portfolioAdd.Document, portfolioAdd.Password);

            return Response(resultResponse);
        }

        /// <summary>
        /// Import Biscoint
        /// </summary>
        /// <returns></returns>
        [HttpPost("biscoint")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncBiscoint([FromBody] PortfolioAddVM portfolioAdd)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = _portfolioApp.ImportBiscoint(portfolioAdd.Document, portfolioAdd.Password);

            return Response(resultResponse);
        }

        /// <summary>
        /// Import BitPreco
        /// </summary>
        /// <returns></returns>
        [HttpPost("bitpreco")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncBitPreco([FromBody] PortfolioAddVM portfolioAdd)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = _portfolioApp.ImportBitPreco(portfolioAdd.Document, portfolioAdd.Password);

            return Response(resultResponse);
        }

        /// <summary>
        /// Import Avenue
        /// </summary>
        /// <returns></returns>
        [HttpPost("avenue")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncAvenue([FromBody] AvenueAddRequest avenueAddRequest)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<AvenueAddResponse> resultResponse = await _brokerIntegrationApp.AuthenticateOnAvenue(avenueAddRequest);

            return Response(resultResponse);
        }

        /// <summary>
        /// Import Avenue (used between server to avoid requests blocks)
        /// </summary>
        /// <returns></returns>
        [HttpPost("avenue-internal")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncAvenueInternal([FromBody] AvenueAddRequest avenueAddRequest)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<AvenueAddResponse> resultResponse = await _brokerIntegrationApp.AuthenticateOnAvenueInternal(avenueAddRequest);

            return Response(resultResponse);
        }

        /// <summary>
        /// Validate Avenue (2FA)
        /// </summary>
        /// <returns></returns>
        [HttpPut("avenue-2fa")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncAvenue2FA([FromBody] Avenue2FARequest avenue2FARequest)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = await _portfolioApp.ImportFromAvenue(avenue2FARequest);

            return Response(resultResponse);
        }

        /// <summary>
        /// Validate Avenue (2FA) (used between server to avoid requests blocks)
        /// </summary>
        /// <returns></returns>
        [HttpPut("avenue-2fa-internal")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncAvenue2FAInternal([FromBody] Avenue2FARequest avenue2FARequest)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = await _portfolioApp.ImportFromAvenueInternal(avenue2FARequest);

            return Response(resultResponse);
        }

        /// <summary>
        /// Import Toro
        /// </summary>
        /// <returns></returns>
        [HttpPost("toro")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncToro([FromBody] ToroAddRequest toroAddRequest)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<ToroAddResponse> resultResponse = _brokerIntegrationApp.AuthenticateOnToro(toroAddRequest);

            return Response(resultResponse);
        }

        /// <summary>
        /// Validate Toro (2FA)
        /// </summary>
        /// <returns></returns>
        [HttpPut("toro-2fa")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncToro2FA([FromBody] ToroAddRequest toroAddRequest)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = _brokerIntegrationApp.ImportFromToro(toroAddRequest);

            return Response(resultResponse);
        }

        /// <summary>
        /// Import NuInvest
        /// </summary>
        /// <returns></returns>
        [HttpPost("nuinvest")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncNuInvest([FromBody] NuInvestAddRequest nuInvestAddRequest)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = _brokerIntegrationApp.ImportFromNuInvest(nuInvestAddRequest);

            return Response(resultResponse);
        }

        /// <summary>
        /// Import NuInvest Internal
        /// </summary>
        /// <returns></returns>
        [HttpPost("nuinvest-internal")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncNuInvestInternal([FromBody] NuInvestAddRequest nuInvestAddRequest)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = _brokerIntegrationApp.ImportFromNuInvestInternal(nuInvestAddRequest);

            return Response(resultResponse);
        }

        /// <summary>
        /// OAuth URL
        /// </summary>
        /// <returns></returns>
        [HttpPost("linkauthuserb3")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetLinkAuthUserB3([FromBody] string document)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<Autorize> resultResponse = _brokerIntegrationApp.PortalInvestidorB3AutorizeUser(document);

            return Response(resultResponse);
        }

        /// <summary>
        /// Import Xp
        /// </summary>
        /// <returns></returns>
        [HttpPost("xp")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncXp([FromBody] XpAddRequest xpAddRequest)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = await _brokerIntegrationApp.ImportFromXpAsync(xpAddRequest);

            return Response(resultResponse);
        }

        /// <summary>
        /// Import Xp Internal
        /// </summary>
        /// <returns></returns>
        [HttpPost("xp-internal")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncXpInternal([FromBody] XpAddRequest xpAddRequest)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = await _brokerIntegrationApp.ImportFromXpInternalAsync(xpAddRequest);

            return Response(resultResponse);
        }


        /// <summary>
        /// Check CEI B3
        /// </summary>
        /// <returns></returns>
        [HttpPost("b3-platform-check")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CheckB3Platform()
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<B3Platform> resultResponse = await _brokerIntegrationApp.CheckB3Platform();

            return Response(resultResponse);
        }

        /// <summary>
        /// Import Rico
        /// </summary>
        /// <returns></returns>
        [HttpPost("rico")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncRico([FromBody] RicoAddRequest ricoAddRequest)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = _brokerIntegrationApp.ImportFromRico(ricoAddRequest);

            return Response(resultResponse);
        }

        /// <summary>
        /// Import Rico Internal
        /// </summary>
        /// <returns></returns>
        [HttpPost("rico-internal")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncRicoInternal([FromBody] RicoAddRequest ricoAddRequest)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<TraderVM> resultResponse = _brokerIntegrationApp.ImportFromRicoInternal(ricoAddRequest);

            return Response(resultResponse);
        }


        /// <summary>
        /// Import Clear
        /// </summary>
        /// <returns></returns>
        [HttpPost("clear")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncClear([FromBody] ClearAddRequest clearAddRequest)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            _brokerIntegrationApp.ImportFromClear(clearAddRequest);

            return Response();
        }

        /// <summary>
        /// Import Clear
        /// </summary>
        /// <returns></returns>
        [HttpPost("clear-internal")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncClearInternal([FromBody] ClearAddRequest clearAddRequest)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            _brokerIntegrationApp.ImportFromClearInternal(clearAddRequest);

            return Response();
        }

        /// <summary>
        /// Import Clear Test
        /// </summary>
        /// <returns></returns>
        [HttpPost("clear-test")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SyncClearTest([FromBody] ClearAddRequest clearAddRequest)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            _brokerIntegrationApp.ImportFromClear(clearAddRequest.IdUser, clearAddRequest.Identifier, clearAddRequest.BirthDate, clearAddRequest.Password);

            return Response();
        }
    }
}
