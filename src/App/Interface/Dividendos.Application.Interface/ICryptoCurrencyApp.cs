
using Dividendos.API.Model.Request.Crypto;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Crypto;
using Dividendos.API.Model.Response.v2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Application.Interface
{
    public interface ICryptoCurrencyApp
    {
        void SyncCryptoCurrencyPrice();

        ResultResponseObject<CryptoPortfolioStatementWrapperVM> GetCryptosByTrader(Guid? traderGuid);

        ResultResponseObject<IEnumerable<CryptoBrokerVM>> GetCryptosBroker();

        ResultResponseObject<IEnumerable<CryptoInfoVM>> GetCryptosInfo(string symbolOrName);

        ResultResponseObject<IEnumerable<API.Model.Response.CryptoMarketMoverVM>> GetMarketMoverByType(CryptoMakertMoversType cryptoMakertMoversType);

        ResultResponseObject<FinancialProductAddVM> UpdateAveragePrice(FinancialProductAddVM financialProductAddVM);

        ResultResponseObject<IEnumerable<API.Model.Response.v2.CryptoMarketMoverVM>> GetMarketMoverByTypeV2(CryptoMakertMoversType cryptoMakertMoversType);

        ResultResponseObject<API.Model.Response.v2.Crypto.CryptoPortfolioStatementWrapperVM> GetCryptosByTraderV2(Guid? traderGuid);
    }
}
