using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dividendos.IexAPIsHelper.Interface.Model;

namespace Dividendos.IexAPIsHelper.Interface
{
	public interface IIexAPIsHelper
	{
		List<DividendCalendarItem> GetDividendCalendar(string symbol);
	}
}
