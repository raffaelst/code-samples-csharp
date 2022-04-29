using Dividendos.BitPreco.Interface;
using Dividendos.BitPreco.Interface.Model;
using K.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Dividendos.BitPreco
{
	public class BitPrecoHelper : IBitPrecoHelper
	{
		public Root GetUserPosition(string apiKey, string apiSecret, ILogger logger)
		{
			Root balances = new Root();

			try
			{
				var request = (HttpWebRequest)WebRequest.Create("https://api.bitpreco.com/trading/");

				Request requestData = new Request() { auth_token = apiSecret + apiKey, cmd = "balance" };

				var data = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(requestData));

				request.Method = "POST";
				request.ContentType = "application/json";
				request.ContentLength = data.Length;
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36";
				request.Accept = "*/*";

				using (var stream = request.GetRequestStream())
				{
					stream.Write(data, 0, data.Length);
				}

				var response = (HttpWebResponse)request.GetResponse();

				var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

				balances = JsonConvert.DeserializeObject<Root>(responseString);
			}
			catch (Exception ex)
			{
				logger.SendErrorAsync(ex);
			}

			return balances;

		}

	}
}

