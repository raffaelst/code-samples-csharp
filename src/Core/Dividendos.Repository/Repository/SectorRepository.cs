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
    public class SectorRepository : Repository<Sector>, ISectorRepository
    {
        private IUnitOfWork _unitOfWork;

        public SectorRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Sector> GetByIdStock(long idStock)
        {
            string sql = @"select Sector.IdSector, Sector.Name, Sector.GuidSector from sector 
                            inner join Subsector on Subsector.IdSector = sector.IdSector 
                            inner join segment on Segment.IdSubsector = Subsector.IdSubsector 
                            inner join Company on Company.IdSegment = Segment.IdSegment 
                            inner join Stock on Stock.IdCompany = Company.IdCompany 
                            where Stock.IdStock = @IdStock ";

            var sectors = _unitOfWork.Connection.Query<Sector>(sql, new { IdStock = idStock }, _unitOfWork.Transaction);

            return sectors;
        }
    }
}
