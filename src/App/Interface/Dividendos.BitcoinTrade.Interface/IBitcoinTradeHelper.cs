using Dividendos.BitcoinTrade.Interface;
using Dividendos.BitcoinTrade.Interface.Model;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.BitcoinTrade
{
	public interface IBitcoinTradeHelper
	{
		Root GetUserPosition(string apiKey, ILogger logger);
	}
}

