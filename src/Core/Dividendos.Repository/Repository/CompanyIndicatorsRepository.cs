using Dapper.Contrib.Extensions;
using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.GenericRepository;
using Dividendos.Repository.Interface.UoW;
using Dapper;
using System.Linq;
using System.Collections.Generic;

namespace Dividendos.Repository.Repository
{
    public class CompanyIndicatorsRepository : Repository<CompanyIndicators>, ICompanyIndicatorsRepository
    {
        private IUnitOfWork _unitOfWork;

        public CompanyIndicatorsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
