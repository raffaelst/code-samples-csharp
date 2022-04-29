using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface IPortfolioPerformanceRepository : IRepository<PortfolioPerformance>
    {
        PortfolioPerformance GetByCalculationDate(long idPortfolio);
        PortfolioPerformance GetByCalculationDate(Guid guidPortfolio);
        IEnumerable<PortfolioPerformance> GetByRangeDate(long idPortfolio);
        PortfolioPerformance GetPreviousDate(long idPortfolio, DateTime calculationDate);
        IEnumerable<PortfolioPerformance> GetByMonth(long idPortfolio, int month, bool first);
        IEnumerable<PortfolioPerformance> GetByYear(long idPortfolio, int year, bool first);
        IEnumerable<PortfolioPerformance> GetByPortfolioSkipHoliday(long idPortfolio, DateTime? startDate, DateTime? endDate);
    }
}
