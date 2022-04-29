using Dividendos.Coinbase.Interface;
using Dividendos.Coinbase.Interface.Model;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Coinbase
{
	public interface ICoinbaseHelper
	{
		List<BalanceResponse> GetUserPosition(string apiKey, string apiSecret, ILogger logger);
	}
}

