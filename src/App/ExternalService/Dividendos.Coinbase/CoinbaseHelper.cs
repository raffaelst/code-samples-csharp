using Coinbase;
using Dividendos.Coinbase.Interface;
using Dividendos.Coinbase.Interface.Model;
using K.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace Dividendos.Coinbase
{
	public class CoinbaseHelper : ICoinbaseHelper
	{
		public List<BalanceResponse> GetUserPosition(string apiKey, string apiSecret, ILogger logger)
		{
			List<BalanceResponse> balances = new List<BalanceResponse>();

			try
            {
				var client = new CoinbaseClient(new ApiKeyConfig { ApiKey = apiKey, ApiSecret = apiSecret });

				var accounts = client.Accounts.ListAccountsAsync().Result;

				if (accounts.Data != null)
				{
					foreach (var item in accounts.Data)
					{
						balances.Add(new BalanceResponse() { asset = item.Balance.Currency, free = item.Balance.Amount });
					}
				}
			}
			catch (Exception ex)
			{
				logger.SendErrorAsync(ex);
			}

			return balances;

		}
	}
}

