using Dividendos.API.Model.Request.Indicator;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IIndicatorApp
    {
        void ImportIndicators();

        ResultResponseObject<IEnumerable<IndicatorVM>> GetAll();
        void ImportInvestingIndicators();
        ResultResponseObject<IEnumerable<IndicatorVM>> GetIndicatorByType(IndicatorType indicatorType);
    }
}
