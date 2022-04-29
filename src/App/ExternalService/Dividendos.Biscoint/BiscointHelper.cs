using Dividendos.Biscoint.Interface;
using Dividendos.Biscoint.Interface.Model;
using K.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Dividendos.Biscoint
{
	public class BiscointHelper : IBiscointHelper
	{
		public Root GetUserPosition(string apiKey, string apiSecret, ILogger logger)
		{
			Root balances = new Root();

			try
            {
				var nonce = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
				var signature = GetSignature(string.Concat("v1/balance", nonce, "\"{}\""), apiSecret);


				using (var httpClient = new HttpClient())
				{
					using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.biscoint.io/v1/balance"))
					{
						request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
						request.Headers.TryAddWithoutValidation("BSCNT-APIKEY", apiKey);
						request.Headers.TryAddWithoutValidation("BSCNT-NONCE", nonce.ToString());
						request.Headers.TryAddWithoutValidation("BSCNT-SIGN", signature);

						var response = httpClient.SendAsync(request).Result;

						if (response.IsSuccessStatusCode)
						{
							var responseContent = response.Content.ReadAsStringAsync().Result;

							var jsonSerializerSettings = new JsonSerializerSettings();
							jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
							balances = JsonConvert.DeserializeObject<Root>(responseContent, jsonSerializerSettings);
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

		public static string GetSignature(string strToBeSigned, string cypherkey)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(strToBeSigned);
			var endpointInBase64 = System.Convert.ToBase64String(plainTextBytes);

			// change according to your needs, an UTF8Encoding
			// could be more suitable in certain situations
			ASCIIEncoding encoding = new ASCIIEncoding();

			Byte[] textBytes = encoding.GetBytes(endpointInBase64);
			Byte[] keyBytes = encoding.GetBytes(cypherkey);

			Byte[] hashBytes;

			using (HMACSHA384 hash = new HMACSHA384(keyBytes))
				hashBytes = hash.ComputeHash(textBytes);

			return Convert.ToBase64String(hashBytes);
		}
	}
}

