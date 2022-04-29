using Dividendos.IexAPIsHelper.Interface;
using Dividendos.IexAPIsHelper.Interface.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.IexAPIsHelper
{
	public class IexAPIsHelper : IIexAPIsHelper
	{
		public List<DividendCalendarItem> GetDividendCalendar(string symbol)
		{
			try
			{
				List<DividendCalendarItem> dividendCalendarItems = new List<DividendCalendarItem>();

				var handler = new HttpClientHandler();
				handler.UseCookies = false;

				using (var httpClient = new HttpClient(handler))
				{
					using (var request = new HttpRequestMessage(new HttpMethod("GET"), String.Format("https://cloud.iexapis.com/stable/stock/{0}/dividends/3m?token=pk_a02a123f252747fd800461ed68a1a9a4", symbol)))
					{
						request.Headers.TryAddWithoutValidation("Cookie", "ctoken=81e709adb91c4a94919feaca738e1c4f");

						var response = httpClient.SendAsync(request).Result;
						IEnumerable<Root> returnFromStatusInvest = JsonConvert.DeserializeObject<IEnumerable<Root>>(response.Content.ReadAsStringAsync().Result);

						if (response != null)
						{
							foreach (var itemCalendar in returnFromStatusInvest)
							{
								dividendCalendarItems.Add(new DividendCalendarItem() { Ticker = itemCalendar.symbol, PaymentDate = itemCalendar.paymentDate, DividendExDate = itemCalendar.exDate, AnnouncementDate = itemCalendar.declaredDate, Value = itemCalendar.amount.ToString("F6", CultureInfo.InvariantCulture) });
							}
						}
					}
				}

				return dividendCalendarItems;

			}
			catch (Exception ex)
			{
				throw new Exception(string.Concat("Nasdaq response not ok - ", ex.Message));
			}
		}
	}
}

