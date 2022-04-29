using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Bounds;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.FinancialProducts;
using Dividendos.Application.Interface.Model;
using System;
using System.Collections.Generic;


namespace Dividendos.Application.Interface
{
    public interface IFinancialProductsApp
    {
        ResultResponseObject<IEnumerable<FinancialProductDetailVM>> GetAllByLoggedUser();

        ResultResponseObject<StockTypeChart> GetCryptosChartsByLoggedUser();

        ResultResponseObject<StockTypeChart> GetFixedIncomeChartsByLoggedUser();
        ResultResponseObject<StockTypeChart> GetBoundsChartsByLoggedUser();

        ResultResponseObject<FinancialProductCryptoVM> GetCryptosByLoggedUser();

        ResultResponseObject<IEnumerable<FinancialProductDetailVM>> GetTDByLoggedUser();

        ResultResponseObject<IEnumerable<FinancialInstitutionVM>> GetAllFinancialInstitution(string name);

        ResultResponseObject<IEnumerable<FinancialProductCategoryVM>> GetAllFinancialProductCategory();

        ResultResponseObject<IEnumerable<FinancialProductVM>> GetAllFinancialProductByCategory(string financialProductCategoryGuid);

        ResultResponseObject<FinancialProductResponse> AddNewProductUser(FinancialProductAddVM financialProductAddVM);

        ResultResponseBase DeleteProductUser(string ProductUserGuid);

        ResultResponseObject<FinancialProductResponse> UpdateProductUser(FinancialProductEditVM financialProductEditVM);

        ResultResponseObject<BoundsSummaryVM> GetBoundsSummaryByLoggedUser();

        ResultResponseObject<BoundsWrapperVM> GetBoundsExtractByLoggedUser();
        ResultResponseObject<BoundsSummaryVM> GetFixedIncomeSummaryByLoggedUser();
        ResultResponseObject<BoundsWrapperVM> GetFixedIncomeExtracByLoggedUser();

        void UpdateListOfBounds();


    }
}
