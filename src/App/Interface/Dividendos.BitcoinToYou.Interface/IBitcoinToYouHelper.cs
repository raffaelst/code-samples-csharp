using Dividendos.BitcoinToYou.Interface;
using Dividendos.BitcoinToYou.Interface.Model;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.BitcoinToYou
{
	public interface IBitcoinToYouHelper
	{
		Root GetUserPosition(string apiKey, string apiSecret, ILogger logger);
	}
}

