using Dividendos.Binance.Interface;
using Dividendos.Binance.Interface.Model;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Binance
{
	public interface IBinanceHelper
	{
		List<BalanceResponse> GetUserPosition(string apiKey, string apiSecret, ILogger logger);

		List<Ticker> GetQuotationOfCryptos();
	}
}

