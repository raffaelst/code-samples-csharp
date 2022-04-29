using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dividendos.Nasdaq.Interface.Model;

namespace Dividendos.Nasdaq.Interface
{
	public interface INasdaqHelper
	{
		List<DividendCalendarItem> GetDividendCalendar(DateTime dateTimeGetData);
	}
}
