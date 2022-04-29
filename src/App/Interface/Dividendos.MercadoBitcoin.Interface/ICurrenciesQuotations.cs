using Dividendos.MercadoBitcoin.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.MercadoBitcoin.Interface
{
	public interface ICurrenciesQuotations
	{
		Task<IEnumerable<Ticker>> GetAsync();
	}
}
