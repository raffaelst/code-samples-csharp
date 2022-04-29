using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IFiatCurrencyService : IBaseService
    {
        ResultServiceObject<IEnumerable<FiatCurrency>> GetAll();
        string GetCurrencySymbol(FiatCurrencyEnum fiatCurrencyEnum);
    }
}
