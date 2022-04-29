using Dividendos.InvestidorB3.Interface.Model;
using Dividendos.InvestidorB3.Interface.Model.Response;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dividendos.InvestidorB3.Interface
{
    public interface IImportInvestidorB3Helper
    {
        string GetAutorizationToken();
        void Healthcheck(string bearer);
        string GetURLAuthB3();

        Model.Response.UpdatedProduct.Root UdpateProduct(string bearer, Model.Request.Product investidorB3Product, DateTime startReferenceDate, DateTime endReferenceDate, int page);
        Model.Response.Collateral.Root Collaterals(string bearer, string documentNumber, DateTime referenceDate, int page);
        Model.Response.ProvisionedEvent.Root ProvisionedEvents(string bearer, string documentNumber, DateTime referenceDate, int page);
        Model.Response.PublicOffer.Root PublicOffers(string bearer, string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, int page);
        Model.Response.AssetTrading.Root AssetsTrading(string bearer, string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, int page);
        Model.Response.DerivativesPosition.Root PositionDerivatives(string bearer, string documentNumber, DateTime referenceDate, int page);
        Model.Response.FixedIncomePosition.Root PositionFixedIncome(string bearer, string documentNumber, DateTime referenceDate, int page);
        Model.Response.EquitiesPosition.Root PositionEquities(string bearer, string documentNumber, DateTime referenceDate, int page);
        Model.Response.TreasuryBondsPosition.Root PositionTreasuryBonds(string bearer, string documentNumber, DateTime referenceDate, int page);
        Model.Response.SecurityLendingPosition.Root PositionSecuritiesLending(string bearer, string documentNumber, DateTime referenceDate, int page);
        Model.Response.SecurityLendingMovement.Root MovementSecuritiesLending(string bearer, string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, int page);
        Model.Response.TreasuryBondsMovement.Root MovementTreasuryBonds(string bearer, string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, int page);
        Model.Response.EquitiesMovement.Root MovementEquities(string bearer, string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, int page);
        Model.Response.FixedIncomeMovement.Root MovementFixedIncome(string bearer, string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, int page);
        Model.Response.DerivativeMovement.Root MovementDerivaties(string bearer, string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, int page);
        ImportCeiResult ImportAllInvestments(string documentNumber, bool automaticProcess, DateTime endReferenceDate, DateTime? lastEventDate, DateTime? lastSync, CancellationTokenSource cancellationTokenSource = null);
        List<string> CheckProductsUpdate(DateTime startDate, DateTime endDate);
        Tuple<List<StockOperation>, List<DividendImport>> GetHistoricalEvents(string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, string token);
    }
}
