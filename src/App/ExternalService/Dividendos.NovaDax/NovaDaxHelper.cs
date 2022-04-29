using Dividendos.NovaDax.Interface;
using Dividendos.NovaDax.Interface.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace Dividendos.NovaDax
{
	public class NovaDaxHelper : INovaDaxHelper
	{
		public Root GetUserPosition(string apiKey, string apiSecret)
		{
			Root responseFromNovaDax = null;
			try
            {
				using (var httpClient = new HttpClient())
				{
					using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.novadax.com/v1/account/getBalance"))
					{
						var response = httpClient.SendAsync(request).Result;

						if (response.IsSuccessStatusCode)
						{
							responseFromNovaDax = JsonConvert.DeserializeObject<Root>(response.Content.ToString());
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(string.Concat("dsaf ", ex.Message));
			}

			return responseFromNovaDax;
		}
	}
}

