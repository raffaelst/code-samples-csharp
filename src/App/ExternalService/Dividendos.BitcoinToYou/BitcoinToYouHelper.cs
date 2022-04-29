using Dividendos.BitcoinToYou.Interface;
using Dividendos.BitcoinToYou.Interface.Model;
using K.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Dividendos.BitcoinToYou
{
	public class BitcoinToYouHelper : IBitcoinToYouHelper
	{
		public Root GetUserPosition(string apiKey, string apiSecret, ILogger logger)
		{
			Root balances = new Root();

			try
			{
				var nonce = "123456789";
				var message = nonce + apiKey;
				var signature = GetHash(message, apiSecret);


				using (var httpClient = new HttpClient())
				{
					using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://back.bitcointoyou.com/api/v2/balance"))
					{
						request.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
						request.Headers.TryAddWithoutValidation("key", apiKey);
						request.Headers.TryAddWithoutValidation("nonce", nonce);
						request.Headers.TryAddWithoutValidation("signature", signature);

						var response = httpClient.SendAsync(request).Result;

						if (response.IsSuccessStatusCode)
						{
							var responseContent = response.Content.ReadAsStringAsync().Result;

							var jsonSerializerSettings = new JsonSerializerSettings();
							jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
							balances = JsonConvert.DeserializeObject<Root>(responseContent, jsonSerializerSettings);
						}
						else
						{
							balances.InvalidCredencial = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				logger.SendErrorAsync(ex);
			}

			return balances;

		}

		public static String GetHash(String str, String cypherkey)
		{
			// change according to your needs, an UTF8Encoding
			// could be more suitable in certain situations
			ASCIIEncoding encoding = new ASCIIEncoding();

			Byte[] textBytes = encoding.GetBytes(str);
			Byte[] keyBytes = encoding.GetBytes(cypherkey);

			Byte[] hashBytes;

			using (HMACSHA256 hash = new HMACSHA256(keyBytes))
				hashBytes = hash.ComputeHash(textBytes);

			return Convert.ToBase64String(hashBytes);
		}

	}
}

