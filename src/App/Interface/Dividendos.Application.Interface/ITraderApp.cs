using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface ITraderApp
    {
        ResultResponseObject<Trader> GetTraderById(long idTrader);

        ResultResponseObject<IEnumerable<TraderSummaryVM>> GetByLoggedUser();

        ResultResponseObject<TraderVM> ChangeTraderCredentials(string identifier, string password, TraderTypeEnum traderTypeEnun);

        void SendAlertToTraderBlocked();

        ResultResponseObject<TraderVM> GetBlockedTrader();

        ResultResponseObject<IEnumerable<API.Model.Response.v4.TraderSummaryVM>> GetByLoggedUserV4();

        ResultResponseObject<TraderVM> Delete(Guid traderGuid);

        ResultResponseObject<IEnumerable<API.Model.Response.v5.TraderSummaryVM>> GetByLoggedUserV5();

        ResultResponseObject<IEnumerable<API.Model.Response.v6.TraderSummaryVM>> GetByLoggedUserV6();
        ResultResponseObject<IEnumerable<API.Model.Response.v7.TraderSummaryVM>> GetByLoggedUserV7();
        ResultResponseObject<TraderVM> CreateNewB3Trader(string documentNumber);
    }
}
