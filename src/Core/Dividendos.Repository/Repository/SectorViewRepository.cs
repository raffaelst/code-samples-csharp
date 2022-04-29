using Dapper;
using Dividendos.Entity.Views;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Repository
{
    public class SectorViewRepository : Repository<SectorView>, ISectorViewRepository
    {
        private IUnitOfWork _unitOfWork;

        public SectorViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<SectorView> GetSumIdPortfolioPerformance(long idPortfolioPerformance)
        {
            string sql = @"select Sector.IdSector, Sector.Name, Sector.GuidSector,sum(PerformanceStock.TotalMarket) TotalMarket from sector 
                            inner join Subsector on Subsector.IdSector = sector.IdSector 
                            inner join segment on Segment.IdSubsector = Subsector.IdSubsector 
                            inner join Company on Company.IdSegment = Segment.IdSegment 
                            inner join Stock on Stock.IdCompany = Company.IdCompany 
                            inner join Operation on Operation.IdStock = Stock.IdStock 
                            inner join PerformanceStock on PerformanceStock.IdStock = Stock.IdStock 
                            inner join PortfolioPerformance on PortfolioPerformance.IdPortfolioPerformance = PerformanceStock.IdPortfolioPerformance
                            where PerformanceStock.IdPortfolioPerformance = @IdPortfolioPerformance 
                            and PortfolioPerformance.IdPortfolio = Operation.IdPortfolio
                            and Operation.IdOperationType = 1 and Operation.Active = 1
                            group by Sector.IdSector, Sector.Name, Sector.GuidSector ";

            var sectors = _unitOfWork.Connection.Query<SectorView>(sql, new { IdPortfolioPerformance = idPortfolioPerformance }, _unitOfWork.Transaction);

            return sectors;
        }

        public IEnumerable<SectorView> GetSumSubPortfolioPerformance(long idPortfolioPerformance, long idSubportfolio)
        {
            string sql = @"select Sector.IdSector, Sector.Name, Sector.GuidSector,sum(PerformanceStock.TotalMarket) TotalMarket from sector 
                            inner join Subsector on Subsector.IdSector = sector.IdSector 
                            inner join segment on Segment.IdSubsector = Subsector.IdSubsector 
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
                            group by Sector.IdSector, Sector.Name, Sector.GuidSector ";

            var sectors = _unitOfWork.Connection.Query<SectorView>(sql, new { IdPortfolioPerformance = idPortfolioPerformance, IdSubPortfolio = idSubportfolio }, _unitOfWork.Transaction);

            return sectors;
        }

    }
}
