using Dapper;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Views;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Repository
{
    public class SegmentViewRepository : Repository<SegmentView>, ISegmentViewRepository
    {
        private IUnitOfWork _unitOfWork;

        public SegmentViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<SegmentView> GetSumIdPortfolioPerformance(long idPortfolioPerformance)
        {
            string sql = @"select Segment.IdSegment,Segment.IdSubsector, Segment.Name, Segment.GuidSegment,sum(PerformanceStock.TotalMarket) TotalMarket from segment 
                            inner join Company on Company.IdSegment = Segment.IdSegment 
                            inner join Stock on Stock.IdCompany = Company.IdCompany 
                            inner join Operation on Operation.IdStock = Stock.IdStock
                            inner join PerformanceStock on PerformanceStock.IdStock = Stock.IdStock 
                            inner join PortfolioPerformance on PortfolioPerformance.IdPortfolioPerformance = PerformanceStock.IdPortfolioPerformance
                            where PerformanceStock.IdPortfolioPerformance = @IdPortfolioPerformance 
                            and PortfolioPerformance.IdPortfolio = Operation.IdPortfolio
                            and Operation.IdOperationType = 1 and Operation.Active = 1
                            group by Segment.IdSegment,Segment.IdSubsector, Segment.Name, Segment.GuidSegment 
                             ";

            var segments = _unitOfWork.Connection.Query<SegmentView>(sql, new { IdPortfolioPerformance = idPortfolioPerformance }, _unitOfWork.Transaction);

            return segments;
        }

        public IEnumerable<SegmentView> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio)
        {
            string sql = @"select Segment.IdSegment,Segment.IdSubsector, Segment.Name, Segment.GuidSegment,sum(PerformanceStock.TotalMarket) TotalMarket from segment 
                            inner join Company on Company.IdSegment = Segment.IdSegment 
                            inner join Stock on Stock.IdCompany = Company.IdCompany 
                            inner join PerformanceStock on PerformanceStock.IdStock = Stock.IdStock 
							inner join PortfolioPerformance on PortfolioPerformance.IdPortfolioPerformance = PerformanceStock.IdPortfolioPerformance 
							inner join SubPortfolio on SubPortfolio.IdPortfolio = PortfolioPerformance.IdPortfolio 
							inner join SubPortfolioOperation on SubPortfolioOperation.IdSubPortfolio = SubPortfolio.IdSubPortfolio 
							inner join Operation on Operation.IdOperation = SubPortfolioOperation.IdOperation 
                            where PerformanceStock.IdPortfolioPerformance = @IdPortfolioPerformance 
							and SubPortfolio.IdSubPortfolio = @IdSubPortfolio 
							and PerformanceStock.IdStock = Operation.IdStock  
                            and Operation.IdOperationType = 1 and Operation.Active = 1 
                            group by Segment.IdSegment,Segment.IdSubsector, Segment.Name, Segment.GuidSegment 
                             ";

            var segments = _unitOfWork.Connection.Query<SegmentView>(sql, new { IdPortfolioPerformance = idPortfolioPerformance, IdSubPortfolio = idSubportfolio }, _unitOfWork.Transaction);

            return segments;
        }

        public IEnumerable<SegmentView> GetSumIdPortfolioPerformance(long idPortfolioPerformance, StockTypeEnum stockTypeEnum)
        {
            string sql = @"select Segment.IdSegment,Segment.IdSubsector, Segment.Name, Segment.GuidSegment,sum(PerformanceStock.TotalMarket) TotalMarket from segment 
                            inner join Company on Company.IdSegment = Segment.IdSegment 
                            inner join Stock on Stock.IdCompany = Company.IdCompany 
                            inner join Operation on Operation.IdStock = Stock.IdStock
                            inner join PerformanceStock on PerformanceStock.IdStock = Stock.IdStock 
                            inner join PortfolioPerformance on PortfolioPerformance.IdPortfolioPerformance = PerformanceStock.IdPortfolioPerformance
                            where PerformanceStock.IdPortfolioPerformance = @IdPortfolioPerformance 
                            and PortfolioPerformance.IdPortfolio = Operation.IdPortfolio
                            and Operation.IdOperationType = 1 and Operation.Active = 1 and Stock.IdStockType = @IdStockType
                            group by Segment.IdSegment,Segment.IdSubsector, Segment.Name, Segment.GuidSegment 
                             ";

            var segments = _unitOfWork.Connection.Query<SegmentView>(sql, new { IdPortfolioPerformance = idPortfolioPerformance, IdStockType = (int)stockTypeEnum }, _unitOfWork.Transaction);

            return segments;
        }

        public IEnumerable<SegmentView> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio, StockTypeEnum stockTypeEnum)
        {
            string sql = @"select Segment.IdSegment,Segment.IdSubsector, Segment.Name, Segment.GuidSegment,sum(PerformanceStock.TotalMarket) TotalMarket from segment 
                            inner join Company on Company.IdSegment = Segment.IdSegment 
                            inner join Stock on Stock.IdCompany = Company.IdCompany 
                            inner join PerformanceStock on PerformanceStock.IdStock = Stock.IdStock 
							inner join PortfolioPerformance on PortfolioPerformance.IdPortfolioPerformance = PerformanceStock.IdPortfolioPerformance 
							inner join SubPortfolio on SubPortfolio.IdPortfolio = PortfolioPerformance.IdPortfolio 
							inner join SubPortfolioOperation on SubPortfolioOperation.IdSubPortfolio = SubPortfolio.IdSubPortfolio 
							inner join Operation on Operation.IdOperation = SubPortfolioOperation.IdOperation 
                            where PerformanceStock.IdPortfolioPerformance = @IdPortfolioPerformance 
							and SubPortfolio.IdSubPortfolio = @IdSubPortfolio 
							and PerformanceStock.IdStock = Operation.IdStock  
                            and Operation.IdOperationType = 1 and Operation.Active = 1 and Stock.IdStockType = @IdStockType
                            group by Segment.IdSegment,Segment.IdSubsector, Segment.Name, Segment.GuidSegment 
                             ";

            var segments = _unitOfWork.Connection.Query<SegmentView>(sql, new { IdPortfolioPerformance = idPortfolioPerformance, IdSubPortfolio = idSubportfolio, IdStockType = (int)stockTypeEnum }, _unitOfWork.Transaction);

            return segments;
        }

    }
}
