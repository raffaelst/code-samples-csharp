using Dapper.Contrib.Extensions;
using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.GenericRepository;
using Dividendos.Repository.Interface.UoW;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using Dividendos.Entity.Views;
using System;
using Dividendos.Entity.Enum;

namespace Dividendos.Repository.Repository
{
    public class CompanyIndicatorsViewRepository : Repository<CompanyIndicatorsView>, ICompanyIndicatorsViewRepository
    {
        private IUnitOfWork _unitOfWork;

        public CompanyIndicatorsViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CompanyIndicatorsView> GetCompanyIndicators(Guid guidStock)
        {
            string sql = @"select distinct Stock.IdStock, Stock.IdStockType, Company.Name Company, stock.Symbol, case when Company.IdCountry = 1 then 'Brasil' else 'Estados Unidos' end as Country, Sector.Name Sector, Subsector.Name Subsector, Segment.Name Segment, Logo.URL Logo,CompanyIndicators.IdCompanyIndicators ,CompanyIndicators.CompanyCode ,CompanyIndicators.ReferenceDate ,CompanyIndicators.NetWorth ,CompanyIndicators.TotalAssets ,CompanyIndicators.NetDebt,CompanyIndicators.NetProfitAnnual,CompanyIndicators.RoeAnnual,CompanyIndicators.RoaAnnual,CompanyIndicators.PricePerVpa,CompanyIndicators.QttyStock,CompanyIndicators.MarketCap,CompanyIndicators.RoicAnnual,CompanyIndicators.PayoutAnnual,CompanyIndicators.Dividend12Months,CompanyIndicators.Dividend12MonthsYield,CompanyIndicators.Dividend24Months,CompanyIndicators.Dividend24MonthsYield,CompanyIndicators.Dividend36Months,CompanyIndicators.Dividend36MonthsYield,CompanyIndicators.PricePerProfit,CompanyIndicators.VlPatrimonyQuotas,CompanyIndicators.TotalQuotaHolder from Company
                                left join CompanyIndicators on Company.IdCompanyIndicators = CompanyIndicators.IdCompanyIndicators
                                inner join Stock on Stock.IdCompany = Company.IdCompany
                                inner join Logo on Company.IdLogo = Logo.IdLogo
                                inner join Segment on segment.IdSegment = Company.IdSegment
                                inner join Subsector on Subsector.IdSubsector = Segment.IdSubsector
                                inner join Sector on Sector.IdSector = Subsector.IdSector
                                where Stock.GuidStock = @GuidStock
                                ";

            IEnumerable<CompanyIndicatorsView> companyView = _unitOfWork.Connection.Query<CompanyIndicatorsView>(sql, new { GuidStock = guidStock }, _unitOfWork.Transaction);

            return companyView;
        }

        public IEnumerable<CompanyIndicatorsView> GetAllByUser(string userID)
        {
            string sql = @"SELECT Company.Name AS CompanyName, RelevantFact.URL, RelevantFact.ReferenceDate
                          FROM RelevantFact 
                          INNER JOIN Company ON Company.IdCompany = RelevantFact.CompanyID
                          INNER JOIN Stock ON Stock.IdCompany = Company.IdCompany AND Stock.ShowOnPortolio = 1
                          INNER JOIN Operation ON Operation.IdStock = Stock.IdStock AND Operation.Active = 1
                          INNER JOIN Portfolio ON Portfolio.IdPortfolio = Operation.IdPortfolio AND Portfolio.Active = 1
                          INNER JOIN Trader ON Trader.IdTrader = Portfolio.IdTrader AND Trader.Active = 1
                          WHERE Trader.IdUser = @IdUser
                          AND RelevantFact.ReferenceDate BETWEEN @CreatedDateStart AND @CreatedDateEnd
                          GROUP BY RelevantFact.URL, Company.Name, RelevantFact.ReferenceDate
                          ORDER BY RelevantFact.ReferenceDate DESC";

            var relevantFacts = _unitOfWork.Connection.Query<CompanyIndicatorsView>(sql, new { IdUser = userID }, _unitOfWork.Transaction);

            return relevantFacts;
        }

        public IEnumerable<CompanyIndicatorsView> GetAll()
        {
            string sql = @"select distinct Stock.IdStock, Stock.IdStockType, Company.Name Company, stock.Symbol, case when Company.IdCountry = 1 then 'Brasil' else 'Estados Unidos' end as Country, Sector.Name Sector, Subsector.Name Subsector, Segment.Name Segment, Logo.URL Logo,CompanyIndicators.IdCompanyIndicators ,CompanyIndicators.CompanyCode ,CompanyIndicators.ReferenceDate ,CompanyIndicators.NetWorth ,CompanyIndicators.TotalAssets ,CompanyIndicators.NetDebt,CompanyIndicators.NetProfitAnnual,CompanyIndicators.RoeAnnual,CompanyIndicators.RoaAnnual,CompanyIndicators.PricePerVpa,CompanyIndicators.QttyStock,CompanyIndicators.MarketCap,CompanyIndicators.RoicAnnual,CompanyIndicators.PayoutAnnual,CompanyIndicators.Dividend12Months,CompanyIndicators.Dividend12MonthsYield,CompanyIndicators.Dividend24Months,CompanyIndicators.Dividend24MonthsYield,CompanyIndicators.Dividend36Months,CompanyIndicators.Dividend36MonthsYield,CompanyIndicators.PricePerProfit,CompanyIndicators.VlPatrimonyQuotas,CompanyIndicators.TotalQuotaHolder from Company
                                left join CompanyIndicators on Company.IdCompanyIndicators = CompanyIndicators.IdCompanyIndicators
                                inner join Stock on Stock.IdCompany = Company.IdCompany
                                inner join Logo on Company.IdLogo = Logo.IdLogo
                                inner join Segment on segment.IdSegment = Company.IdSegment
                                inner join Subsector on Subsector.IdSubsector = Segment.IdSubsector
                                inner join Sector on Sector.IdSector = Subsector.IdSector
                                ";

            IEnumerable<CompanyIndicatorsView> companyView = _unitOfWork.Connection.Query<CompanyIndicatorsView>(sql,  null, _unitOfWork.Transaction);

            return companyView;
        }

        public IEnumerable<CompanyIndicatorsView> GetAllByType(StockTypeEnum stockTypeEnum)
        {
            string sql = @"select distinct Stock.IdStock, Stock.IdStockType, Company.Name Company, stock.Symbol, case when Company.IdCountry = 1 then 'Brasil' else 'Estados Unidos' end as Country, Sector.Name Sector, Subsector.Name Subsector, Segment.Name Segment, Logo.URL Logo,CompanyIndicators.IdCompanyIndicators ,CompanyIndicators.CompanyCode ,CompanyIndicators.ReferenceDate ,CompanyIndicators.NetWorth ,CompanyIndicators.TotalAssets ,CompanyIndicators.NetDebt,CompanyIndicators.NetProfitAnnual,CompanyIndicators.RoeAnnual,CompanyIndicators.RoaAnnual,CompanyIndicators.PricePerVpa,CompanyIndicators.QttyStock,CompanyIndicators.MarketCap,CompanyIndicators.RoicAnnual,CompanyIndicators.PayoutAnnual,CompanyIndicators.Dividend12Months,CompanyIndicators.Dividend12MonthsYield,CompanyIndicators.Dividend24Months,CompanyIndicators.Dividend24MonthsYield,CompanyIndicators.Dividend36Months,CompanyIndicators.Dividend36MonthsYield,CompanyIndicators.PricePerProfit,CompanyIndicators.VlPatrimonyQuotas,CompanyIndicators.TotalQuotaHolder from Company
                                left join CompanyIndicators on Company.IdCompanyIndicators = CompanyIndicators.IdCompanyIndicators
                                inner join Stock on Stock.IdCompany = Company.IdCompany
                                inner join Logo on Company.IdLogo = Logo.IdLogo
                                inner join Segment on segment.IdSegment = Company.IdSegment
                                inner join Subsector on Subsector.IdSubsector = Segment.IdSubsector
                                inner join Sector on Sector.IdSector = Subsector.IdSector
                                where Stock.IdStockType = @IdStockType
                                ";

            IEnumerable<CompanyIndicatorsView> companyView = _unitOfWork.Connection.Query<CompanyIndicatorsView>(sql, new { IdStockType = (int)stockTypeEnum }, _unitOfWork.Transaction);

            return companyView;
        }

        public IEnumerable<CompanyIndicatorsView> GetAllByUserAndType(string userID, StockTypeEnum stockTypeEnum)
        {
            string sql = @"SELECT Company.Name AS CompanyName, RelevantFact.URL, RelevantFact.ReferenceDate
                          FROM RelevantFact 
                          INNER JOIN Company ON Company.IdCompany = RelevantFact.CompanyID
                          INNER JOIN Stock ON Stock.IdCompany = Company.IdCompany AND Stock.ShowOnPortolio = 1
                          INNER JOIN Operation ON Operation.IdStock = Stock.IdStock AND Operation.Active = 1
                          INNER JOIN Portfolio ON Portfolio.IdPortfolio = Operation.IdPortfolio AND Portfolio.Active = 1
                          INNER JOIN Trader ON Trader.IdTrader = Portfolio.IdTrader AND Trader.Active = 1
                          WHERE Trader.IdUser = @IdUser
                          AND RelevantFact.ReferenceDate BETWEEN @CreatedDateStart AND @CreatedDateEnd
                          GROUP BY RelevantFact.URL, Company.Name, RelevantFact.ReferenceDate
                          ORDER BY RelevantFact.ReferenceDate DESC";

            var relevantFacts = _unitOfWork.Connection.Query<CompanyIndicatorsView>(sql, new { IdUser = userID }, _unitOfWork.Transaction);

            return relevantFacts;
        }
    }
}
