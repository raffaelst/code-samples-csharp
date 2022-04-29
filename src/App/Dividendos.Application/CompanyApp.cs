using AutoMapper;
using K.Logger;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;

using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Finance.Interface;
using Dividendos.Finance.Interface.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dividendos.Application.Interface.Model;
using Dividendos.Entity.Views;
using Dividendos.TradeMap.Interface;
using System.IO;
using Dividendos.Entity.Enum;
using Dividendos.TradeMap.Interface.Model;
using Dividendos.API.Model.Response.Company;

namespace Dividendos.Application
{
    public class CompanyApp : BaseApp, ICompanyApp
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICompanyService _companyService;
        private readonly ITradeMapHelper _iTradeMapHelper;
        private readonly ILogoService _logoService;
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly IStockService _stockService;

        public CompanyApp(IMapper mapper,
            ICompanyService companyService,
            IUnitOfWork uow,
            ITradeMapHelper iTradeMapHelper,
            ILogoService logoService,
            ISystemSettingsService systemSettingsService,
            IStockService stockService)
        {
            _mapper = mapper;
            _uow = uow;
            _companyService = companyService;
            _iTradeMapHelper = iTradeMapHelper;
            _logoService = logoService;
            _systemSettingsService = systemSettingsService;
            _stockService = stockService;
        }

        public ResultResponseObject<IEnumerable<CompanyView>> GetAll()
        {
            ResultResponseObject<IEnumerable<CompanyView>> resultResponseObject = new ResultResponseObject<IEnumerable<CompanyView>>();

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<CompanyView>> resultServiceObject = _companyService.GetAll();

                //resultResponseObject = _mapper.Map<ResultResponseObject<IEnumerable<comp>>>(resultServiceObject);
            }

            return resultResponseObject;
        }

        public ResultResponseObject<IEnumerable<CompanyVM>> GetByName(string name)
        {
            ResultResponseObject<IEnumerable<CompanyVM>> resultResponseObject = new ResultResponseObject<IEnumerable<CompanyVM>>();

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<Company>> resultServiceObject = _companyService.GetByName(name);

                resultResponseObject = _mapper.Map<ResultResponseObject<IEnumerable<CompanyVM>>>(resultServiceObject);
            }

            return resultResponseObject;
        }

        public async Task UpdateCompanyLogo()
        {
            ResultServiceObject<IEnumerable<Company>> resultServiceObject = null;

            using (_uow.Create())
            {
                resultServiceObject = _companyService.GetAllByCountryWithoutLogo(1);
            }


            if (resultServiceObject.Success && resultServiceObject.Value != null && resultServiceObject.Value.Count() > 0)
            {
                foreach (Company company in resultServiceObject.Value)
                {
                    if (company.IdSegment != 90 && company.IdSegment != 91 && company.IdSegment != 92 && company.IdSegment != 93 && company.IdSegment != 94
                         && company.IdSegment != 95 && company.IdSegment != 96 && company.IdSegment != 97 && company.IdSegment != 98 && company.IdSegment != 99
                          && company.IdSegment != 92 && company.IdSegment != 102 && company.IdSegment != 87)
                    {
                        using (_uow.Create())
                        {
                            Logo logo = new Logo();
                            logo.CompanyCode = company.Code;
                            logo.LogoImage = await _iTradeMapHelper.GetLogo64(string.Format("https://portal.trademap.com.br/assets/images/company_logos/{0}.png", company.Code.ToUpper()));
                            logo = _logoService.Insert(logo).Value;

                            company.IdLogo = logo.IdLogo;

                            _companyService.Update(company);
                        }
                    }
                }
            }

        }

        public async Task Generatefiles()
        {
            ResultServiceObject<IEnumerable<Logo>> resultServiceObject = null;

            using (_uow.Create())
            {
                resultServiceObject = _logoService.GetGreater(5691);
            }

            if (resultServiceObject.Success && resultServiceObject.Value != null && resultServiceObject.Value.Count() > 0)
            {
                foreach (Logo logo in resultServiceObject.Value)
                {
                    try
                    {
                        string convert = logo.LogoImage.Replace("data:image/jpeg;base64,", String.Empty);
                        File.WriteAllBytes(string.Format(@"C:\Users\rafael\Downloads\Images\{0}.jpeg", logo.CompanyCode), Convert.FromBase64String(convert));
                    }
                    catch (Exception ex)
                    {

                        continue;
                    }

                }
            }
        }
    }
}