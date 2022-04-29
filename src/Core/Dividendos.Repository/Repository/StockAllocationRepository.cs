using Dapper;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Views;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Dividendos.Repository.Repository
{
    public class StockAllocationRepository : Repository<StockAllocation>, IStockAllocationRepository
    {
        private IUnitOfWork _unitOfWork;

        public StockAllocationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IEnumerable<StockAllocation> GetSumIdPortfolioPerformance(long idPortfolioPerformance)
        {
            string sql = @"select Company.Name Company, Stock.Symbol,sum(PerformanceStock.TotalMarket) TotalMarket 
                            from PerformanceStock 
                            inner join Stock on Stock.IdStock = PerformanceStock.IdStock 
                            inner join Operation on Operation.IdStock = Stock.IdStock
                            inner join Company on Company.IdCompany = Stock.IdCompany 
                            inner join PortfolioPerformance on PortfolioPerformance.IdPortfolioPerformance = PerformanceStock.IdPortfolioPerformance
                            where PerformanceStock.IdPortfolioPerformance = @IdPortfolioPerformance 
							and PortfolioPerformance.IdPortfolio = Operation.IdPortfolio
							and Operation.IdOperationType = 1 and Operation.Active = 1
                            group by Company.Name, Stock.Symbol ";

            var stockAllocation = _unitOfWork.Connection.Query<StockAllocation>(sql, new { IdPortfolioPerformance = idPortfolioPerformance }, _unitOfWork.Transaction);

            return stockAllocation;
        }

        public IEnumerable<StockAllocation> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio)
        {
            string sql = @"select Company.Name Company, Stock.Symbol,sum(PerformanceStock.TotalMarket) TotalMarket 
                            from PerformanceStock 
                            inner join Stock on Stock.IdStock = PerformanceStock.IdStock 
                            inner join Company on Company.IdCompany = Stock.IdCompany 
							inner join PortfolioPerformance on PortfolioPerformance.IdPortfolioPerformance = PerformanceStock.IdPortfolioPerformance
							inner join SubPortfolio on SubPortfolio.IdPortfolio = PortfolioPerformance.IdPortfolio
							inner join SubPortfolioOperation on SubPortfolioOperation.IdSubPortfolio = SubPortfolio.IdSubPortfolio
							inner join Operation on Operation.IdOperation = SubPortfolioOperation.IdOperation
                            where PerformanceStock.IdPortfolioPerformance = @IdPortfolioPerformance  
							and SubPortfolio.IdSubPortfolio = @IdSubPortfolio 
							and PerformanceStock.IdStock = Operation.IdStock 
                            and Operation.IdOperationType = 1 and Operation.Active = 1
                            group by Company.Name, Stock.Symbol ";

            var stockAllocation = _unitOfWork.Connection.Query<StockAllocation>(sql, new { IdPortfolioPerformance = idPortfolioPerformance, IdSubPortfolio = idSubportfolio }, _unitOfWork.Transaction);

            return stockAllocation;
        }

        public IEnumerable<StockAllocation> GetSumPerformanceStock(long idPortfolioPerformance)
        {
            string sql = @"select Company.Name Company, Stock.Symbol,PerformanceStock.TotalMarket TotalMarket,PerformanceStock.PerformancePerc PerformancePerc
                            from PerformanceStock 
                            inner join Stock on Stock.IdStock = PerformanceStock.IdStock 
                            inner join Operation on Operation.IdStock = Stock.IdStock
                            inner join Company on Company.IdCompany = Stock.IdCompany 
                            inner join PortfolioPerformance on PortfolioPerformance.IdPortfolioPerformance = PerformanceStock.IdPortfolioPerformance
                            where PerformanceStock.IdPortfolioPerformance = @IdPortfolioPerformance 
							and PortfolioPerformance.IdPortfolio = Operation.IdPortfolio
							and Operation.IdOperationType = 1 and Operation.Active = 1
                            order by Stock.Symbol ";

            var stockAllocation = _unitOfWork.Connection.Query<StockAllocation>(sql, new { IdPortfolioPerformance = idPortfolioPerformance }, _unitOfWork.Transaction);

            return stockAllocation;
        }

        public IEnumerable<StockAllocation> GetSumSubPerformanceStock(long idPortfolioPerformance, long idSubportfolio)
        {
            string sql = @"select Company.Name Company, Stock.Symbol,PerformanceStock.TotalMarket TotalMarket, PerformanceStock.PerformancePerc 
                            from PerformanceStock 
                            inner join Stock on Stock.IdStock = PerformanceStock.IdStock 
                            inner join Company on Company.IdCompany = Stock.IdCompany 
							inner join PortfolioPerformance on PortfolioPerformance.IdPortfolioPerformance = PerformanceStock.IdPortfolioPerformance 
							inner join SubPortfolio on SubPortfolio.IdPortfolio = PortfolioPerformance.IdPortfolio 
							inner join SubPortfolioOperation on SubPortfolioOperation.IdSubPortfolio = SubPortfolio.IdSubPortfolio 
							inner join Operation on Operation.IdOperation = SubPortfolioOperation.IdOperation 
                            where PerformanceStock.IdPortfolioPerformance = @IdPortfolioPerformance 
							and SubPortfolio.IdSubPortfolio = @IdSubPortfolio 
							and PerformanceStock.IdStock = Operation.IdStock 
                            and Operation.IdOperationType = 1 and Operation.Active = 1
                            order by Stock.Symbol ";

            var stockAllocation = _unitOfWork.Connection.Query<StockAllocation>(sql, new { IdPortfolioPerformance = idPortfolioPerformance, IdSubPortfolio = idSubportfolio }, _unitOfWork.Transaction);

            return stockAllocation;
        }

        public IEnumerable<StockAllocation> GetSumSegmentStock(long idPortfolioPerformance, long idSegment)
        {
            string sql = @"select stock.Symbol,sum(PerformanceStock.TotalMarket) TotalMarket from segment 
                            inner join Company on Company.IdSegment = Segment.IdSegment 
                            inner join Stock on Stock.IdCompany = Company.IdCompany 
                            inner join Operation on Operation.IdStock = Stock.IdStock
                            inner join PerformanceStock on PerformanceStock.IdStock = Stock.IdStock 
                            inner join PortfolioPerformance on PortfolioPerformance.IdPortfolioPerformance = PerformanceStock.IdPortfolioPerformance
                            where PerformanceStock.IdPortfolioPerformance = @IdPortfolioPerformance
                            and PortfolioPerformance.IdPortfolio = Operation.IdPortfolio
                            and Operation.IdOperationType = 1 and Operation.Active = 1
							and Company.IdSegment = @IdSegment
                            group by stock.Symbol ";

            var stockAllocation = _unitOfWork.Connection.Query<StockAllocation>(sql, new { IdPortfolioPerformance = idPortfolioPerformance, IdSegment = idSegment }, _unitOfWork.Transaction);

            return stockAllocation;
        }

        public IEnumerable<StockAllocation> GetSumSubsectorStock(long idPortfolioPerformance, long idSubsector)
        {
            string sql = @"select stock.Symbol,sum(PerformanceStock.TotalMarket) TotalMarket from Subsector  
                            inner join Segment on Segment.IdSubsector = Subsector.IdSubsector 
                            inner join Company on Company.IdSegment = Segment.IdSegment                             
                            inner join Stock on Stock.IdCompany = Company.IdCompany 
                            inner join Operation on Operation.IdStock = Stock.IdStock
                            inner join PerformanceStock on PerformanceStock.IdStock = Stock.IdStock 
                            inner join PortfolioPerformance on PortfolioPerformance.IdPortfolioPerformance = PerformanceStock.IdPortfolioPerformance
                            where PerformanceStock.IdPortfolioPerformance = @IdPortfolioPerformance
                            and PortfolioPerformance.IdPortfolio = Operation.IdPortfolio
                            and Operation.IdOperationType = 1 and Operation.Active = 1
							and Subsector.IdSubsector = @IdSubsector
                            group by stock.Symbol ";

            var stockAllocation = _unitOfWork.Connection.Query<StockAllocation>(sql, new { IdPortfolioPerformance = idPortfolioPerformance, IdSubsector = idSubsector }, _unitOfWork.Transaction);

            return stockAllocation;
        }

        public IEnumerable<StockAllocation> GetSumSectorStock(long idPortfolioPerformance, long idSector)
        {
            string sql = @"select stock.Symbol,sum(PerformanceStock.TotalMarket) TotalMarket from sector  
                            inner join Subsector on sector.IdSector = Subsector.IdSector
                            inner join Segment on Segment.IdSubsector = Subsector.IdSubsector 
                            inner join Company on Company.IdSegment = Segment.IdSegment                             
                            inner join Stock on Stock.IdCompany = Company.IdCompany 
                            inner join Operation on Operation.IdStock = Stock.IdStock
                            inner join PerformanceStock on PerformanceStock.IdStock = Stock.IdStock 
                            inner join PortfolioPerformance on PortfolioPerformance.IdPortfolioPerformance = PerformanceStock.IdPortfolioPerformance
                            where PerformanceStock.IdPortfolioPerformance = @IdPortfolioPerformance
                            and PortfolioPerformance.IdPortfolio = Operation.IdPortfolio
                            and Operation.IdOperationType = 1 and Operation.Active = 1
							and sector.IdSector = @IdSector
                            group by stock.Symbol ";

            var stockAllocation = _unitOfWork.Connection.Query<StockAllocation>(sql, new { IdPortfolioPerformance = idPortfolioPerformance, IdSector = idSector }, _unitOfWork.Transaction);

            return stockAllocation;
        }
    }
}
