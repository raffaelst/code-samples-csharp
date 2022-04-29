using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class FiatCurrencyApp : BaseApp, IFiatCurrencyApp
    {
        private readonly IUnitOfWork _uow;
        private readonly IFiatCurrencyService _fiatCurrencyService;

        public FiatCurrencyApp(IUnitOfWork uow, IFiatCurrencyService fiatCurrencyService)
        {
            _uow = uow;
            _fiatCurrencyService = fiatCurrencyService;
        }

        public ResultResponseObject<IEnumerable<FiatCurrencyVM>> GetAll()
        {
            ResultResponseObject<IEnumerable<FiatCurrencyVM>> responseObject = new ResultResponseObject<IEnumerable<FiatCurrencyVM>>();
            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<FiatCurrency>> resultService = _fiatCurrencyService.GetAll();

                if (resultService.Value != null && resultService.Value.Count() > 0)
                {
                    List<FiatCurrencyVM> fiatCurrencies = new List<FiatCurrencyVM>();

                    foreach (FiatCurrency fiatCurrency in resultService.Value)
                    {
                        fiatCurrencies.Add(new FiatCurrencyVM { IdFiatCurrency = fiatCurrency.IdFiatCurrency, Name = fiatCurrency.Name });
                    }

                    responseObject.Value = fiatCurrencies;
                    responseObject.Success = true;
                }
            }

            return responseObject;
        }
    }
}
