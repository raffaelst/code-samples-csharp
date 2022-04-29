using Dividendos.Entity.Entities;
using Dividendos.TradeMap.Interface;
using Dividendos.TradeMap.Interface.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.TradeMap
{
	public class TradeMapHelper : ITradeMapHelper
	{
		public int count { get; set; }

		public async Task<TradeMapPrices> ImportStockPricesAsync(List<Stock> stocks, string cookie, int idCountry)
		{
			TradeMapPrices tradeMapPrices = null;
			IEnumerable<List<Stock>> stockLstSplit = SplitList(stocks, 50);

			if (stockLstSplit != null && stockLstSplit.Count() > 0)
			{
				tradeMapPrices = new TradeMapPrices();
				//tradeMapPrices.Result = new Result();
				tradeMapPrices.Result = new Dictionary<string, string[]>();
				foreach (List<Stock> stockSplit in stockLstSplit)
				{
					TradeMapPrices tradeMapPricesResult = await ImportPerParts(stockSplit, cookie, idCountry);

					if (tradeMapPricesResult != null && tradeMapPricesResult.Success && tradeMapPricesResult.Result != null && tradeMapPricesResult.Result != null && tradeMapPricesResult.Result.Count > 0)
					{
						foreach (KeyValuePair<string, string[]> keyValuePair in tradeMapPricesResult.Result)
						{
							tradeMapPrices.Result.Add(keyValuePair.Key, keyValuePair.Value);
						}
					}
				}

				tradeMapPrices.Success = true;
			}

			return tradeMapPrices;
		}

		private static async Task<TradeMapPrices> ImportPerParts(List<Stock> stocks, string cookie, int idCountry)
		{
			TradeMapPrices tradeMap = null;

			StringBuilder sbStocks = new StringBuilder();

			if (stocks != null && stocks.Count > 0)
			{
				string sufix = "1";

				if (idCountry == 2)
				{
					sufix = "7777";
				}

				for (int i = 0; i < stocks.Count; i++)
				{
					if (i == stocks.Count - 1)
					{
						sbStocks.Append(string.Format("\"{0}:{1}\"", stocks[i].Symbol, sufix));
					}
					else
					{
						sbStocks.Append(string.Format("\"{0}:{1}\",", stocks[i].Symbol, sufix));
					}
				}
			}

			var handler = new HttpClientHandler();
			handler.UseCookies = true;

			using (var httpClient = new HttpClient(handler))
			{
				using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://portal.trademap.com.br/api/trademap/v1/stock/stock-variation-posi"))
				{
					request.Headers.TryAddWithoutValidation("cookie", cookie);
					request.Content = new StringContent(string.Concat("{\"cd_stocks\":[", sbStocks.ToString(), "],\"tbbar\":\"fiveminutes\",\"with_chart\":false,\"last_price\":true,\"size\":2000}"));
					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					var response = await httpClient.SendAsync(request);

					if (response.IsSuccessStatusCode)
					{
						var responseContent = await response.Content.ReadAsStringAsync();

						var jsonSerializerSettings = new JsonSerializerSettings();
						jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
						tradeMap = JsonConvert.DeserializeObject<TradeMapPrices>(responseContent, jsonSerializerSettings);
					}
					else
					{
						throw new Exception("TM response not ok");
					}
				}
			}

			if (tradeMap == null)
			{
				throw new Exception("TM response not ok");
			}

			return tradeMap;
		}

		public static IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize = 30)
		{
			for (int i = 0; i < locations.Count; i += nSize)
			{
				yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
			}
		}

		public async Task<TradeMapUSAStocks> ImportUsStocks(string cookie)
		{			
			TradeMapUSAStocks tradeMapUSAStocks = null;
			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://portal.trademap.com.br/api/trademap/v5/fundamental/get-company-search"))
				{
					request.Headers.TryAddWithoutValidation("cookie", cookie);

					request.Content = new StringContent("{\"market\":{\"id_company_position_sector\":0,\"id_company_position_subsector\":0,\"id_company_position_segment\":[]},\"index\":{\"min_vl_mcap\":null,\"max_vl_mcap\":null,\"min_net_worth\":null,\"max_net_worth\":null,\"min_mcap_over_pl\":null,\"max_mcap_over_pl\":null,\"min_current_liquidity\":null,\"max_current_liquidity\":null,\"min_ebit_margin_annual\":null,\"max_ebit_margin_annual\":null,\"min_net_margin_annual\":null,\"max_net_margin_annual\":null,\"min_vl_dividend_yield_annual\":null,\"max_vl_dividend_yield_annual\":null,\"min_roe_annual\":null,\"max_roe_annual\":null,\"min_roa_annual\":null,\"max_roa_annual\":null}}");
					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					var response = await httpClient.SendAsync(request);

					if (response.IsSuccessStatusCode)
					{
						var responseContent = await response.Content.ReadAsStringAsync();

						var jsonSerializerSettings = new JsonSerializerSettings();
						jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
						tradeMapUSAStocks = JsonConvert.DeserializeObject<TradeMapUSAStocks>(responseContent, jsonSerializerSettings);
					}
				}
			}
			if (tradeMapUSAStocks != null && tradeMapUSAStocks.ResultStock != null && tradeMapUSAStocks.ResultStock.Count() > 0)
			{
				foreach (ResultStock resultStock in tradeMapUSAStocks.ResultStock)
				{
					string logo64 = await GetLogo64(resultStock.Logo);

					resultStock.Logo = logo64;
				}
			}

			return tradeMapUSAStocks;
		}

		public async Task<string> GetLogo64(string logo)
		{
			string logo64 = string.Empty;

			try
			{
				using (var client = new HttpClient())
				{
					var bytes = await client.GetByteArrayAsync(logo);
					logo64 = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
				}
			}
			catch (Exception)
			{
				if (count <= 3)
				{
					count++;
					logo64 = await GetLogo64(logo);
				}
			}

			return logo64;
		}

		public TradeMapUSAStocks ImportReistsAndEtfs(string cookie, int idCountry, int idStockType, List<Stock> stocksExclude)
		{
			TradeMapUSAStocks tradeMapUSAStocks = null;
			List<string> stockNotFound = new List<string>();
			var handler = new HttpClientHandler();
			handler.UseCookies = false;

			using (var httpClient = new HttpClient(handler))
			{
				using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://www.nyse.com/api/quotes/filter"))
				{
					request.Headers.TryAddWithoutValidation("Cookie", "__cfduid=dbd50cb809cdd596dd364f924c6ea762a1592573777; JSESSIONID=7C34CD74C60846D08E61EAD3BD9A4E05; ICE=!hBMjE1oogsuw3fqQmW/NR4Un8KL87HfSPuTOWaVLHFVagIWromX0GLewjgsVeA/5WXVGouJrTEv5eA==; TS01ebd031=0100e6d4958b1ad014ece69ccfaa7a2d90fb1f6f325a515daa5d9911a25efedb06d5a33b9e4dcefaaef6f86bb8d651ce51b50c263fe80fefd4674c12325fb4fe2bda8e0ef15582d9560f8a5cb1437db8c971c48db1");

					if (idStockType == 1)
					{
						request.Content = new StringContent("{\n    \"instrumentType\": \"EQUITY\",\n    \"pageNumber\": 1,\n    \"sortColumn\": \"NORMALIZED_TICKER\",\n    \"sortOrder\": \"ASC\",\n    \"maxResultsPerPage\": 20000,\n    \"filterToken\": \"\"\n}");
					}
					else if (idStockType == 2)
					{
						request.Content = new StringContent("{\n    \"instrumentType\": \"REIT\",\n    \"pageNumber\": 1,\n    \"sortColumn\": \"NORMALIZED_TICKER\",\n    \"sortOrder\": \"ASC\",\n    \"maxResultsPerPage\": 20000,\n    \"filterToken\": \"\"\n}");
					}
					else
					{
						request.Content = new StringContent("{\n    \"instrumentType\": \"EXCHANGE_TRADED_FUND\",\n    \"pageNumber\": 1,\n    \"sortColumn\": \"NORMALIZED_TICKER\",\n    \"sortOrder\": \"ASC\",\n    \"maxResultsPerPage\": 20000,\n    \"filterToken\": \"\"\n}");
					}

					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					try
					{
						var response = httpClient.SendAsync(request).Result;

						if (response.IsSuccessStatusCode)
						{
							var responseContent = response.Content.ReadAsStringAsync().Result;

							var jsonSerializerSettings = new JsonSerializerSettings();
							jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
							List<NyseStock> nyseStocks = JsonConvert.DeserializeObject<List<NyseStock>>(responseContent, jsonSerializerSettings);

							if (nyseStocks != null && nyseStocks.Count > 0)
							{
								tradeMapUSAStocks = new TradeMapUSAStocks();
								tradeMapUSAStocks.Success = true;
								tradeMapUSAStocks.ResultStock = new List<ResultStock>();

								List<Stock> stocks = new List<Stock>();

								foreach (NyseStock nyseStock in nyseStocks)
								{
									Stock stockFound = stocksExclude.FirstOrDefault(stc => stc.Symbol == nyseStock.NormalizedTicker.ToUpper());

									if (stockFound == null)
									{
										count = 0;
										stocks.Add(new Stock { Symbol = nyseStock.NormalizedTicker });

										ResultStock resultStock = new ResultStock();
										resultStock.CdCompany = nyseStock.NormalizedTicker.ToUpper();

										if (idStockType == 1)
										{
											resultStock.DsPositionSegment = "Stocks";
										}
										else if (idStockType == 2)
										{
											resultStock.DsPositionSegment = "REITs";
										}
										else
										{
											resultStock.DsPositionSegment = "ETFs";
										}

										resultStock.NmCompanyExchange = nyseStock.InstrumentName;


										TradeCompanyInfo tradeCompanyInfo = GetCompany(resultStock.CdCompany, cookie).Result;

										if (tradeCompanyInfo.Success)
										{
											if (idStockType == 2)
											{
												resultStock.Sector = "REITs";
												resultStock.Industry = "REITs";
											}
											else
											{
												resultStock.Sector = tradeCompanyInfo.Result.MarketPosition.Sector;
												resultStock.Industry = tradeCompanyInfo.Result.MarketPosition.Industry;

												if (string.IsNullOrWhiteSpace(tradeCompanyInfo.Result.MarketPosition.Sector) || string.IsNullOrWhiteSpace(tradeCompanyInfo.Result.MarketPosition.Industry))
                                                {
													if (idStockType == 1)
													{
														resultStock.Sector = "Stocks";
														resultStock.Industry = "Stocks";
													}
													else if (idStockType == 2)
													{
														resultStock.Sector = "REITs";
														resultStock.Industry = "REITs";
													}
													else
													{
														resultStock.Sector = "ETFs";
														resultStock.Industry = "ETFs";
													}
												}
											}

											resultStock.Logo = GetCompanyLogo(resultStock.CdCompany, cookie).Result;

											tradeMapUSAStocks.ResultStock.Add(resultStock);
										}
										else
										{
											if (idStockType == 1)
											{
												resultStock.Sector = "Stocks";
												resultStock.Industry = "Stocks";
											}
											else if (idStockType == 2)
											{
												resultStock.Sector = "REITs";
												resultStock.Industry = "REITs";
											}
											else
											{
												resultStock.Sector = "ETFs";
												resultStock.Industry = "ETFs";
											}

											tradeMapUSAStocks.ResultStock.Add(resultStock);
										}
									}
								}								
							}
						}
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
			}

			return tradeMapUSAStocks;
		}


		public async Task<TradeCompanyInfo> GetCompany(string symbol, string cookie)
		{
			TradeCompanyInfo tradeCompanyInfo = null;

			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://portal.trademap.com.br/api/trademap/v2/fundamental/company-info"))
				{
					request.Headers.TryAddWithoutValidation("cookie", cookie);

					//request.Content = new StringContent("{\"cd_stock\":\"RWT\",\"id_exchange\":7777}");
					string requestBody = string.Concat("{\"cd_stock\":\"", symbol, "\",\"id_exchange\":7777}");
					request.Content = new StringContent(requestBody);
					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					var response = await httpClient.SendAsync(request);

					if (response.IsSuccessStatusCode)
					{
						var responseContent = await response.Content.ReadAsStringAsync();

						var jsonSerializerSettings = new JsonSerializerSettings();
						jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
						tradeCompanyInfo = JsonConvert.DeserializeObject<TradeCompanyInfo>(responseContent, jsonSerializerSettings);

					}
				}
			}

			return tradeCompanyInfo;
		}


		public async Task<string> GetCompanyLogo(string symbol, string cookie)
		{
			string base64 = string.Empty;

			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://portal.trademap.com.br/api/trademap/v1/stock/get-offshore-company-logo"))
				{
					request.Headers.TryAddWithoutValidation("cookie", cookie);

					string requestBody = string.Concat("{\"cd_stock\":\"", symbol, "\",\"id_exchange\":7777}");
					request.Content = new StringContent(requestBody);
					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					var response = await httpClient.SendAsync(request);

					if (response.IsSuccessStatusCode)
					{
						var responseContent = await response.Content.ReadAsStringAsync();

						var jsonSerializerSettings = new JsonSerializerSettings();
						jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
						TradeCompanyLogo tradeCompanyLogo = JsonConvert.DeserializeObject<TradeCompanyLogo>(responseContent, jsonSerializerSettings);


						using (var client = new HttpClient())
						{
							base64 = await GetLogo64(tradeCompanyLogo.ResultCompanyLogo);
							//var bytes = await client.GetByteArrayAsync(tradeCompanyLogo.ResultCompanyLogo);
							//base64 = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
						}
					}
				}
			}

			return base64;
		}



		public async Task<List<StockMarketMover>> ImportMarketMoversAsync(string cookie, string country, string order, string index, string exchange)
		{
			List<StockMarketMover> stockMarketMover = null;

			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://portal.trademap.com.br/api/trademap/v1/marketMovers/get-market-movers-variation-by-code"))
				{
					request.Headers.TryAddWithoutValidation("cookie", cookie);

					request.Content = new StringContent($@"{{'cd_index': {index},'movers_id_exchange': {exchange},'order_by': {order},'period':'oneday','market_type':{country}','limit':'20'}}");

					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					try
					{
						var response = await httpClient.SendAsync(request);

						if (response.IsSuccessStatusCode)
						{
							var responseContent = await response.Content.ReadAsStringAsync();

							var jsonSerializerSettings = new JsonSerializerSettings();
							jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
							StockMarketMoverResult stockMarketMoverResult = JsonConvert.DeserializeObject<StockMarketMoverResult>(responseContent, jsonSerializerSettings);

							stockMarketMover = stockMarketMoverResult.StockMarketMover;
						}
					}
					catch (Exception ex)
					{
						//throw ex;
					}
				}
			}

			return stockMarketMover;
		}


		public async Task<List<StockMarketMover>> ImportMarketMoversDividendPaidAsync(string cookie, string market_type, string index, string exchange)
		{
			List<StockMarketMover> stockMarketMover = null;

			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://portal.trademap.com.br/api/trademap/v1/marketMovers/get-market-movers-dividend-paid-by-code"))
				{
					request.Headers.TryAddWithoutValidation("cookie", cookie);

					request.Content = new StringContent($@"{{'cd_index': {index},'id_exchange':{exchange},'order_by':'desc','market_type': {market_type},'limit':'20'}}");

					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					try
					{
						var response = await httpClient.SendAsync(request);

						if (response.IsSuccessStatusCode)
						{
							var responseContent = await response.Content.ReadAsStringAsync();

							var jsonSerializerSettings = new JsonSerializerSettings();
							jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
							StockMarketMoverResult stockMarketMoverResult = JsonConvert.DeserializeObject<StockMarketMoverResult>(responseContent, jsonSerializerSettings);

							stockMarketMover = stockMarketMoverResult.StockMarketMover;
						}
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
			}

			return stockMarketMover;
		}


		public async Task<List<StockMarketMover>> ImportMarketMoversDividendYieldAsync(string cookie, string market_type, string index, string exchange)
		{
			List<StockMarketMover> stockMarketMover = null;

			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://portal.trademap.com.br/api/trademap/v9/stock-list/get-stock-list-items-dynamic"))
				{
					request.Headers.TryAddWithoutValidation("cookie", cookie);

					request.Content = new StringContent($@"{{'cd_index': {index},'id_exchange':{exchange},'order_by':'desc','market_type': {market_type},'limit':'20'}}");

					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					try
					{
						var response = await httpClient.SendAsync(request);

						if (response.IsSuccessStatusCode)
						{
							var responseContent = await response.Content.ReadAsStringAsync();

							var jsonSerializerSettings = new JsonSerializerSettings();
							jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
							StockMarketMoverResult stockMarketMoverResult = JsonConvert.DeserializeObject<StockMarketMoverResult>(responseContent, jsonSerializerSettings);

							stockMarketMover = stockMarketMoverResult.StockMarketMover;
						}
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
			}

			return stockMarketMover;
		}

		public async Task<List<StockMarketMover>> ImportMarketMoversDividendREITsAsync(string cookie, string market_type)
		{
			List<StockMarketMover> stockMarketMover = null;

			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://portal.trademap.com.br/api/trademap/v9/stock-list/get-stock-list-items-dynamic"))
				{
					request.Headers.TryAddWithoutValidation("cookie", cookie);

					request.Content = new StringContent(market_type);

					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					try
					{
						var response = await httpClient.SendAsync(request);

						if (response.IsSuccessStatusCode)
						{
							var responseContent = await response.Content.ReadAsStringAsync();

							var jsonSerializerSettings = new JsonSerializerSettings();
							jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
							StockMarketMoverResult stockMarketMoverResult = JsonConvert.DeserializeObject<StockMarketMoverResult>(responseContent, jsonSerializerSettings);

							stockMarketMover = stockMarketMoverResult.StockMarketMover;
						}
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
			}

			return stockMarketMover;
		}

		public async Task<List<string>> GetHolidays(string cookie)
        {
			List<string> holidays = new List<string>();

			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://portal.trademap.com.br/api/trademap/carteira/v2/calendar/get-bovespa-holidays"))
				{
					request.Headers.TryAddWithoutValidation("cookie", cookie);

					request.Content = new StringContent("{\"dt_start\":-1,\"dt_end\":-1,\"id_exchange\":1}");
					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					try
					{
						var response = await httpClient.SendAsync(request);

						if (response.IsSuccessStatusCode)
						{
							var responseContent = await response.Content.ReadAsStringAsync();

							var jsonSerializerSettings = new JsonSerializerSettings();
							jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
							holidays = JsonConvert.DeserializeObject<HolidayTd>(responseContent, jsonSerializerSettings).Result;							
						}
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
			}

			return holidays;
		}

		public CompanyIndicatorTd ImportCompanyIndicators(string cookie, string symbol)
        {
			CompanyIndicatorTd companyIndicatorTd = null;
			var client = new RestClient("https://portal.trademap.com.br/api/trademap/v2/fundamental/company-indices-now");
			client.Timeout = -1;
			var request = new RestRequest(Method.POST);
			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("cookie", cookie);
			var body = @"{""cd_stock"":" + symbol + @",""id_exchange"":1}";
			request.AddParameter("application/json", body, ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);

			if (response.IsSuccessful)
			{
				var jsonSerializerSettings = new JsonSerializerSettings();
				jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
				companyIndicatorTd = JsonConvert.DeserializeObject<CompanyIndicatorTd>(response.Content, jsonSerializerSettings);
			}

			return companyIndicatorTd;
		}

		public CompanyHistoricTd ImportCompanyHistoricCorporate(string cookie, string symbol)
        {
			CompanyHistoricTd companyIndicatorTd = null;

			try
			{
				var client = new RestClient("https://portal.trademap.com.br/api/trademap/v1/stock/get-hist-corporate-event-full");
				client.Timeout = -1;
				var request = new RestRequest(Method.POST);
				request.AddHeader("Content-Type", "application/json");
				request.AddHeader("cookie", cookie);
				var body = @"{""cd_stock"":" + symbol + @",""id_exchange"":1}";
				request.AddParameter("application/json", body, ParameterType.RequestBody);
				IRestResponse response = client.Execute(request);

				if (response.IsSuccessful)
				{
					var jsonSerializerSettings = new JsonSerializerSettings();
					jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
					companyIndicatorTd = JsonConvert.DeserializeObject<CompanyHistoricTd>(response.Content, jsonSerializerSettings);
				}

			}
			catch (Exception ex)
            {
				string test = "po";
			}


			return companyIndicatorTd;
		}


		public CompanyIndicatorUsTd ImportCompanyIndicatorsUs(string cookie, string symbol)
		{
			CompanyIndicatorUsTd companyIndicatorTd = null;

			try
			{
				var client = new RestClient("https://portal.trademap.com.br/api/trademap/v5/fundamental/company-us-financial");
				client.Timeout = -1;
				var request = new RestRequest(Method.POST);
				request.AddHeader("Content-Type", "application/json");
				request.AddHeader("cookie", cookie);
				var body = @"{""cd_stock"":" + symbol + @",""id_exchange"":7777}";
				request.AddParameter("application/json", body, ParameterType.RequestBody);
				IRestResponse response = client.Execute(request);

				if (response.IsSuccessful)
				{
					var jsonSerializerSettings = new JsonSerializerSettings();
					jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
					companyIndicatorTd = JsonConvert.DeserializeObject<CompanyIndicatorUsTd>(response.Content, jsonSerializerSettings);
				}
			}
			catch(Exception ex)
            {
				string test = "po";
			}

			return companyIndicatorTd;
		}

		public CompanyIndicatorFiisTd ImportCompanyIndicatorsFiis(string cookie, string symbol)
		{
			CompanyIndicatorFiisTd companyIndicatorTd = null;

			try
			{
				var client = new RestClient("https://portal.trademap.com.br/api/trademap/v7/fii/getFiiIndicesNow");
				client.Timeout = -1;
				var request = new RestRequest(Method.POST);
				request.AddHeader("Content-Type", "application/json");
				request.AddHeader("cookie", cookie);
				var body = @"{""cd_stock"":" + symbol + @",""id_exchange"":1}";
				request.AddParameter("application/json", body, ParameterType.RequestBody);
				IRestResponse response = client.Execute(request);

				if (response.IsSuccessful)
				{
					var jsonSerializerSettings = new JsonSerializerSettings();
					jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
					companyIndicatorTd = JsonConvert.DeserializeObject<CompanyIndicatorFiisTd>(response.Content, jsonSerializerSettings);
				}
			}
			catch (Exception ex)
			{
				string test = "po";
			}

			return companyIndicatorTd;
		}

	}
}

