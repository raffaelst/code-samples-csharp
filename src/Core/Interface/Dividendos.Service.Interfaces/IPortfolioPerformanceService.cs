using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IPortfolioPerformanceService : IBaseService
    {
        ResultServiceObject<PortfolioPerformance> GetByCalculationDate(long idPortfolio);
        ResultServiceObject<PortfolioPerformance> GetByCalculationDate(Guid guidPortfolio);
        ResultServiceObject<PortfolioPerformance> Update(PortfolioPerformance portfolioPerformance);
        ResultServiceObject<PortfolioPerformance> Insert(PortfolioPerformance portfolioPerformance);
        ResultServiceObject<IEnumerable<PortfolioPerformance>> GetByRangeDate(long idPortfolio);
        ResultServiceObject<PortfolioPerformance> GetByCalculationDate(long idPortfolio, DateTime calculationDate);
        ResultServiceObject<PortfolioPerformance> GetPreviousDate(long idPortfolio, DateTime calculationDate);
        ResultServiceObject<PortfolioPerformance> GetByMonth(long idPortfolio, int month, bool first);
        ResultServiceObject<PortfolioPerformance> GetByYear(long idPortfolio, int month, bool first);
        ResultServiceObject<IEnumerable<PortfolioPerformance>> GetByPortfolioSkipHoliday(long idPortfolio, DateTime? startDate, DateTime? endDate);
        ResultServiceObject<IEnumerable<PortfolioPerformance>> GetByIdPortfolio(long idPortfolio);
    }
}
