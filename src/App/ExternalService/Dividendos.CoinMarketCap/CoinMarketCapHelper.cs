using Dividendos.CoinMarketCap.Interface;
using Dividendos.CoinMarketCap.Interface.Model;
using K.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace Dividendos.CoinMarketCap
{
	public class CoinMarketCapHelper : ICoinMarketCapHelper
	{
		public List<TickerCrypto> GetQuotationOfCryptos(string listSymbols, string key)
		{
			List<TickerCrypto> ticker = new List<TickerCrypto>();

			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Concat("https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest?skip_invalid=true&convert=USD&symbol=", listSymbols)))
				{
					request.Headers.TryAddWithoutValidation("X-CMC_PRO_API_KEY", key);
					request.Headers.TryAddWithoutValidation("Accept", "application/json");

					var response = httpClient.SendAsync(request).Result;


                    if (response.IsSuccessStatusCode)
                    {
                        dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(response.Content.ReadAsStringAsync().Result);

                        IDictionary<string, object> propertiesFromApi = resultAPI;

                        foreach (var property in propertiesFromApi)
                        {
                            if (property.Key.Equals("data"))
                            {
                                dynamic cryptoDetails = property.Value;

                                foreach (var itemCryptoDetail in cryptoDetails)
                                {
                                    var itemCryptoDetailValue = itemCryptoDetail.Value;

                                    TickerCrypto tickerCrypto = new TickerCrypto();

                                    foreach (var itemCrypto in itemCryptoDetailValue)
                                    {
                                        if (itemCrypto.Key.Equals("symbol"))
                                        {
                                            tickerCrypto.Symbol = itemCrypto.Value;
                                        }
                                        else if (itemCrypto.Key.Equals("name"))
                                        {
                                            tickerCrypto.Name = itemCrypto.Value;
                                        } 
                                        else if (itemCrypto.Key.Equals("quote"))
                                        {
                                            dynamic value = itemCrypto.Value;

                                            try
                                            {
                                                tickerCrypto.Price = (decimal)value.USD.price;
                                                tickerCrypto.Volume24h = (decimal)value.USD.volume_24h;
                                                tickerCrypto.PercentChange1h = (decimal)value.USD.percent_change_1h;
                                                tickerCrypto.PercentChange24h = (decimal)value.USD.percent_change_24h;
                                                tickerCrypto.PercentChange7d = (decimal)value.USD.percent_change_7d;
                                                tickerCrypto.PercentChange30d = (decimal)value.USD.percent_change_30d;
                                                tickerCrypto.PercentChange60d = (decimal)value.USD.percent_change_60d;
                                                tickerCrypto.PercentChange90d = (decimal)value.USD.percent_change_90d;
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }

                                    ticker.Add(tickerCrypto);
                                }
                            }
                        }
                    }
                }
			}

			return ticker;
		}


        public List<Tuple<string, string, string, byte[]>> GetLogo(string symbol)
        {
            List<Tuple<string, string, string, byte[]>> logos = new List<Tuple<string, string, string, byte[]>>();

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Concat("https://pro-api.coinmarketcap.com/v1/cryptocurrency/info?symbol=", symbol)))
                {
                    request.Headers.TryAddWithoutValidation("X-CMC_PRO_API_KEY", "94130164-4089-412c-8015-fe331dcd1c0b");
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");

                    var response = httpClient.SendAsync(request).Result;


                    if (response.IsSuccessStatusCode)
                    {
                        dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(response.Content.ReadAsStringAsync().Result);

                        IDictionary<string, object> propertiesFromApi = resultAPI;

                        foreach (var property in propertiesFromApi)
                        {
                            if (property.Key.Equals("data"))
                            {
                                dynamic cryptoDetails = property.Value;

                                foreach (var itemCryptoDetail in cryptoDetails)
                                {
                                    var itemCryptoDetailValue = itemCryptoDetail.Value;

                                    string image = "", symbolResponse = "", nameResponse = "";
                                    byte[] imageArray = null;

                                    foreach (var itemCrypto in itemCryptoDetailValue)
                                    {
                                        if (itemCrypto.Key.Equals("logo"))
                                        {
                                            using (var client = new HttpClient())
                                            {
                                                imageArray = client.GetByteArrayAsync(itemCrypto.Value).Result;

                                                image = "data:image/jpeg;base64," + Convert.ToBase64String(imageArray);
                                            }
                                        }

                                        if (itemCrypto.Key.Equals("symbol"))
                                        {
                                            symbolResponse = itemCrypto.Value;
                                        }

                                        if (itemCrypto.Key.Equals("name"))
                                        {
                                            nameResponse = itemCrypto.Value;
                                        }
                                    }

                                    logos.Add(new Tuple<string, string, string, byte[]>(symbol, image, nameResponse, imageArray));
                                }
                            }
                        }
                    }
                }
            }

            return logos;
        }
    }
}

