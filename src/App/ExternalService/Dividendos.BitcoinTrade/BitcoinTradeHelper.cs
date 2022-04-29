using Dividendos.BitcoinTrade.Interface;
using Dividendos.BitcoinTrade.Interface.Model;
using K.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace Dividendos.BitcoinTrade
{
	public class BitcoinTradeHelper : IBitcoinTradeHelper
	{
		public Root GetUserPosition(string apiKey, ILogger logger)
		{
			Root balance = new Root();

			try
            {
				using (var httpClient = new HttpClient())
				{
					using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.bitcointrade.com.br/v3/wallets/balance"))
					{
						request.Headers.TryAddWithoutValidation("x-api-key", apiKey);

						var response = httpClient.SendAsync(request).Result;


						if (response.IsSuccessStatusCode)
						{
							balance = JsonConvert.DeserializeObject<Root>(response.Content.ReadAsStringAsync().Result);
						}
						else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
						{
							balance.code = "invalidApiKey";
						}
					}
				}
			}
			catch (Exception ex)
			{
				logger.SendErrorAsync(ex);
			}

			return balance;

		}
	}
}

