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
    public class SegmentRepository : Repository<Segment>, ISegmentRepository
    {
        private IUnitOfWork _unitOfWork;

        public SegmentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
