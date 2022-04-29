using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service
{
    public class FiatCurrencyService : BaseService, IFiatCurrencyService
    {
        public FiatCurrencyService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<IEnumerable<FiatCurrency>> GetAll()
        {
            ResultServiceObject<IEnumerable<FiatCurrency>> resultService = new ResultServiceObject<IEnumerable<FiatCurrency>>();

            IEnumerable<FiatCurrency> fiatCurrencies = _uow.FiatCurrencyRepository.GetAll();

            resultService.Value = fiatCurrencies;

            return resultService;
        }

        public string GetCurrencySymbol(FiatCurrencyEnum fiatCurrencyEnum)
        {
            string currencySymbol = string.Empty;

            switch (fiatCurrencyEnum)
            {
                case FiatCurrencyEnum.BRL:
                    currencySymbol = "R$";
                    break;
                case FiatCurrencyEnum.USD:
                    currencySymbol = "$";
                    break;
                case FiatCurrencyEnum.EURO:
                    currencySymbol = "€";
                    break;
                default:
                    break;
            }

            return currencySymbol;
        }
    }
}
