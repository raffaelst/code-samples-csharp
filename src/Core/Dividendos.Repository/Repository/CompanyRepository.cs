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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private IUnitOfWork _unitOfWork;

        public CompanyRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Company> GetByName(string name)
        {
            string sql = @"select * from Company WHERE Company.Name like '' + @Name + '%' OR Company.FullName like '' + @Name + '%'";

            var financialInstitutions = _unitOfWork.Connection.Query<Company>(sql, new { Name = name }, _unitOfWork.Transaction);

            return financialInstitutions;
        }

        public IEnumerable<Company> GetByGuid(string companyGuid)
        {
            string sql = @"select * from Company WHERE GuidCompany = @GuidCompany";

            var financialInstitutions = _unitOfWork.Connection.Query<Company>(sql, new { GuidCompany = companyGuid }, _unitOfWork.Transaction);

            return financialInstitutions;
        }
    }
}
