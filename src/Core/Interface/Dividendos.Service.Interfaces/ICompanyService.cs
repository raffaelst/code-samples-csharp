using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface ICompanyService : IBaseService
    {
        ResultServiceObject<CompanyView> GetCompanyLogoDetails(long idStock);
        ResultServiceObject<IEnumerable<CompanyView>> GetAll();

        ResultServiceObject<IEnumerable<Company>> GetAllByCountry(int idCountry);
        ResultServiceObject<Company> Update(Company company);
        ResultServiceObject<Company> Insert(Company company);
        ResultServiceObject<IEnumerable<Company>> GetAllByCountryWithoutLogo(int idCountry);
        ResultServiceObject<Company> GetById(long idCompany);
        ResultServiceObject<IEnumerable<Company>> GetByName(string name);
        ResultServiceObject<Company> GetByGuid(string guidCompany);
    }
}
