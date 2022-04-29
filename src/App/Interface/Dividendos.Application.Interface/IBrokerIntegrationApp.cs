using Dividendos.API.Model.Request.BrokerIntegration;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.BrokerIntegration;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.v1.PortalInvestidorB3;
using Dividendos.Application.Interface.Model;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.NuInvest.Interface.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IBrokerIntegrationApp
    {
        ResultResponseObject<PassfolioAddResponse> AuthenticateOnPassfolio(PassfolioAddRequest passfolioAddRequest);
        Task<ResultResponseObject<AvenueAddResponse>> AuthenticateOnAvenue(AvenueAddRequest avenueAddRequest);
        Task<ResultResponseObject<AvenueAddResponse>> AuthenticateOnAvenueInternal(AvenueAddRequest avenueAddRequest);
        ResultResponseObject<ToroAddResponse> AuthenticateOnToro(ToroAddRequest toroAddRequest);
        ResultResponseObject<TraderVM> ImportFromToro(string email, string password, string token, string idUser);
        ResultResponseObject<TraderVM> ImportFromToro(ToroAddRequest toroAddRequest);
        ResultResponseObject<TraderVM> ImportFromNuInvest(string idUser, string identifier, string token, string password);
        ResultResponseObject<TraderVM> ImportFromNuInvest(NuInvestAddRequest nuInvestAddRequest);
        Task<ResultResponseObject<TraderVM>> ImportFromXptAsync(string idUser, string account, string password, string xpToken);
        void TesteInvestidorB3();
        ResultResponseObject<Autorize> PortalInvestidorB3AutorizeUser(string document);
        Task<ResultResponseObject<TraderVM>> ImportFromXpAsync(XpAddRequest xpAddRequest);
        Task<ResultResponseObject<TraderVM>> ImportFromXpInternalAsync(XpAddRequest xpAddRequest);
        void SyncB3AssetsTrading();
        void TestNewCei();
        Task<ResultResponseObject<B3Platform>> CheckB3Platform();
        ResultResponseObject<TraderVM> ImportFromRico(string idUser, string account, string password, string token);
        ResultResponseObject<TraderVM> ImportFromRico(RicoAddRequest ricoAddRequest);
        ResultResponseObject<TraderVM> ImportFromRicoInternal(RicoAddRequest ricoAddRequest);
        ResultResponseObject<TraderVM> ImportFromClear(string idUser, string identifier, string birthDate, string password);
        ResultResponseObject<TraderVM> ImportFromClear(ClearAddRequest clearAddRequest);
        ResultResponseObject<TraderVM> ImportFromClearInternal(ClearAddRequest clearAddRequest);
        ResultResponseObject<TraderVM> ImportFromNuInvestInternal(NuInvestAddRequest nuInvestAddRequest);
    }
}
