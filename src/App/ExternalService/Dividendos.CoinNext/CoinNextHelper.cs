using Dividendos.CoinNext.Interface;
using Dividendos.CoinNext.Interface.Model;
using K.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace Dividendos.CoinNext
{
	public class CoinNextHelper : ICoinNextHelper
	{
		public List<BalanceResponse> GetUserPosition(string apiKey, string apiSecret, ILogger logger)
		{
			List<BalanceResponse> balances = new List<BalanceResponse>();

			try
            {
				
			}
			catch (Exception ex)
			{
				logger.SendErrorAsync(ex);
			}

			return balances;

		}
	}
}

