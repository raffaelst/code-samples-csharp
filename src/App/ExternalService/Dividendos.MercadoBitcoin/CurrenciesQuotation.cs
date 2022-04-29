using Dividendos.MercadoBitcoin.Interface;
using Dividendos.MercadoBitcoin.Interface.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.MercadoBitcoin
{
	public class CurrenciesQuotations : ICurrenciesQuotations
	{
		public CurrenciesQuotations()
		{
	
		}

	
		public async Task<IEnumerable<Ticker>> GetAsync()
		{
			List<Ticker> tickers = new List<Ticker>();

			var handler = new HttpClientHandler();
			handler.UseCookies = false;

			using (var httpClient = new HttpClient(handler))
			{
				//using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://www.mercadobitcoin.net/api/btc/ticker/"))
				//{
				//	var response = await httpClient.SendAsync(request);

				//	Ticker ticker = JsonConvert.DeserializeObject<TickerResponse>(response.Content.ReadAsStringAsync().Result).Ticker;
				//	ticker.name = "btc";
				//	tickers.Add(ticker);
				//}

				//using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://www.mercadobitcoin.net/api/ltc/ticker/"))
				//{
				//	var response = await httpClient.SendAsync(request);

				//	Ticker ticker = JsonConvert.DeserializeObject<TickerResponse>(response.Content.ReadAsStringAsync().Result).Ticker;
				//	ticker.name = "ltc";

				//	tickers.Add(ticker);
				//}

				//using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://www.mercadobitcoin.net/api/bch/ticker/"))
				//{
				//	var response = await httpClient.SendAsync(request);

				//	Ticker ticker = JsonConvert.DeserializeObject<TickerResponse>(response.Content.ReadAsStringAsync().Result).Ticker;
				//	ticker.name = "bch";

				//	tickers.Add(ticker);
				//}

				//using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://www.mercadobitcoin.net/api/eth/ticker/"))
				//{
				//	var response = await httpClient.SendAsync(request);

				//	Ticker ticker = JsonConvert.DeserializeObject<TickerResponse>(response.Content.ReadAsStringAsync().Result).Ticker;
				//	ticker.name = "eth";

				//	tickers.Add(ticker);
				//}

				using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://www.mercadobitcoin.net/api/xrp/ticker/"))
				{
					var response = await httpClient.SendAsync(request);

					Ticker ticker = JsonConvert.DeserializeObject<TickerResponse>(response.Content.ReadAsStringAsync().Result).Ticker;
					ticker.name = "xrp";

					tickers.Add(ticker);
				}

				using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://www.mercadobitcoin.net/api/usdc/ticker/"))
				{
					var response = await httpClient.SendAsync(request);

					Ticker ticker = JsonConvert.DeserializeObject<TickerResponse>(response.Content.ReadAsStringAsync().Result).Ticker;
					ticker.name = "usdc";

					tickers.Add(ticker);
				}

				using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://www.mercadobitcoin.net/api/wbx/ticker/"))
				{
					var response = await httpClient.SendAsync(request);

					Ticker ticker = JsonConvert.DeserializeObject<TickerResponse>(response.Content.ReadAsStringAsync().Result).Ticker;
					ticker.name = "wbx";

					tickers.Add(ticker);
				}

				
			}

			return tickers;
		}
		
	}
}
