using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dividendos.Service
{
    public class CompanyService : BaseService, ICompanyService
    {
        public CompanyService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<CompanyView> GetCompanyLogoDetails(long idStock)
        {
            ResultServiceObject<CompanyView> resultService = new ResultServiceObject<CompanyView>();

            IEnumerable<CompanyView> companyView = _uow.CompanyViewRepository.GetCompanyLogoDetails(idStock);

            resultService.Value = companyView.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<IEnumerable<CompanyView>> GetAll()
        {
            ResultServiceObject<IEnumerable<CompanyView>> resultService = new ResultServiceObject<IEnumerable<CompanyView>>();

            IEnumerable<CompanyView> companyView = _uow.CompanyViewRepository.GetAll();

            resultService.Value = companyView;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<Company>> GetAllByCountry(int idCountry)
        {
            ResultServiceObject<IEnumerable<Company>> resultService = new ResultServiceObject<IEnumerable<Company>>();

            IEnumerable<Company> companyView = _uow.CompanyRepository.Select(p => p.IdCountry == idCountry);

            resultService.Value = companyView;

            return resultService;
        }

        public ResultServiceObject<Company> Update(Company company)
        {
            ResultServiceObject<Company> resultService = new ResultServiceObject<Company>();
            resultService.Value = _uow.CompanyRepository.Update(company);

            return resultService;
        }

        public ResultServiceObject<Company> Insert(Company company)
        {
            ResultServiceObject<Company> resultService = new ResultServiceObject<Company>();
            company.GuidCompany = Guid.NewGuid();
            company.IdCompany = _uow.CompanyRepository.Insert(company);
            resultService.Value = company;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<Company>> GetAllByCountryWithoutLogo(int idCountry)
        {
            ResultServiceObject<IEnumerable<Company>> resultService = new ResultServiceObject<IEnumerable<Company>>();

            //IEnumerable<Company> companyView = _uow.CompanyRepository.Select(p => p.IdCountry == idCountry && p.IdLogo.HasValue && p.IdLogo.Value == 552);

            IEnumerable<Company> companyView = _uow.CompanyRepository.Select(p => p.IdCountry == idCountry && p.IdLogo != 552 && p.IdLogo <= 5691);

            resultService.Value = companyView;

            return resultService;
        }

        public ResultServiceObject<Company> GetById(long idCompany)
        {
            ResultServiceObject<Company> resultService = new ResultServiceObject<Company>();

            IEnumerable<Company> companyView = _uow.CompanyRepository.Select(p => p.IdCompany == idCompany);

            resultService.Value = companyView.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<Company> GetByGuid(string guidCompany)
        {
            ResultServiceObject<Company> resultService = new ResultServiceObject<Company>();

            IEnumerable<Company> companyView = _uow.CompanyRepository.GetByGuid(guidCompany);

            resultService.Value = companyView.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<IEnumerable<Company>> GetByName(string name)
        {
            ResultServiceObject<IEnumerable<Company>> resultService = new ResultServiceObject<IEnumerable<Company>>();

            IEnumerable<Company> companyView = _uow.CompanyRepository.GetByName(name);

            resultService.Value = companyView;

            return resultService;
        }
    }
}
