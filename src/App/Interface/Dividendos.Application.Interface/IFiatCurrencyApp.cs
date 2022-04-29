using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IFiatCurrencyApp
    {
        ResultResponseObject<IEnumerable<FiatCurrencyVM>> GetAll();
    }
}
