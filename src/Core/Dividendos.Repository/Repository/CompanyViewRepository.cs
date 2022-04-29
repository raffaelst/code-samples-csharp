using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using Dividendos.Entity.Views;
using System.Dynamic;

namespace Dividendos.Repository.Repository
{
    public class CompanyViewRepository : Repository<CompanyView>, ICompanyViewRepository
    {
        private IUnitOfWork _unitOfWork;

        public CompanyViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CompanyView> GetCompanyLogoDetails(long idStock)
        {
            string sql = @"select distinct Stock.IdStock, Company.Name Company, stock.Symbol, Segment.Name Segment, Logo.LogoImage Logo
                            from Stock
                            inner join Company on Company.IdCompany = Stock.IdCompany
                            inner join Logo on logo.IdLogo = Company.IdLogo
                            inner join Segment on segment.IdSegment = Company.IdSegment
                            where Stock.IdStock = @IdStock	 ";

            IEnumerable<CompanyView> companyView = _unitOfWork.Connection.Query<CompanyView>(sql, new { IdStock = idStock }, _unitOfWork.Transaction);

            return companyView;
        }
    }
}
