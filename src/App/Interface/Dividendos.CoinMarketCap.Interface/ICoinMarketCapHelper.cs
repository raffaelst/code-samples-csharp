using Dividendos.CoinMarketCap.Interface;
using Dividendos.CoinMarketCap.Interface.Model;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.CoinMarketCap
{
	public interface ICoinMarketCapHelper
	{
		List<TickerCrypto> GetQuotationOfCryptos(string listSymbols, string key);

		List<Tuple<string, string, string, byte[]>> GetLogo(string symbol);
	}
}

