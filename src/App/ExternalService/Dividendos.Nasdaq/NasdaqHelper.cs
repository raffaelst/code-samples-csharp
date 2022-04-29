using Dividendos.Nasdaq.Interface;
using Dividendos.Nasdaq.Interface.Model;
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

namespace Dividendos.Nasdaq
{
	public class NasdaqHelper : INasdaqHelper
	{
		private const string PROXY_IP = "172.31.1.215";

		public List<DividendCalendarItem> GetDividendCalendar(DateTime dateTimeGetData)
		{
			try
			{
				List<DividendCalendarItem> dividendCalendarItems = new List<DividendCalendarItem>();

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Concat("https://api.nasdaq.com/api/calendar/dividends?date=", dateTimeGetData.ToString("yyyy-MM-dd")));
				//request.Proxy = new WebProxy(string.Format("{0}:{1}", PROXY_IP, "21218"), false);
				request.KeepAlive = true;
				request.Headers.Add("sec-ch-ua", @"""Google Chrome"";v=""95"", ""Chromium"";v=""95"", "";Not A Brand"";v=""99""");
				request.Headers.Add("sec-ch-ua-mobile", @"?0");
				request.Headers.Add("sec-ch-ua-platform", @"""Windows""");
				request.Headers.Add("DNT", @"1");
				request.Headers.Add("Upgrade-Insecure-Requests", @"1");
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36";
				request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
				request.Headers.Add("Sec-Fetch-Site", @"none");
				request.Headers.Add("Sec-Fetch-Mode", @"navigate");
				request.Headers.Add("Sec-Fetch-User", @"?1");
				request.Headers.Add("Sec-Fetch-Dest", @"document");
				request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
				request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9,pt-BR;q=0.8,pt;q=0.7,de;q=0.6");
				request.Headers.Set(HttpRequestHeader.Cookie, @"_mkto_trk=id:303-QKM-463&token:_mch-nasdaq.com-1622488633183-87971; _hjid=1de61908-24a0-4817-9b1a-d2afa52c467f; recentlyViewedList=NOBL|ETF,VO|ETF,VOO|ETF; _ga_4VZJSZ76VC=GS1.1.1622494238.3.1.1622495688.60; _ga=GA1.2.455880162.1622488631; _uetvid=cd7b4a20c24411eb955f9d83349faaa6; __gads=ID=2b401e9689f3ca7c:T=1622495707:S=ALNI_MZQeWXVGhpWf5jpd3xB1K8Ad0JJ7Q");

				var response = (HttpWebResponse)request.GetResponse();

				using (Stream responseStream = response.GetResponseStream())
				{
					Stream streamToRead = responseStream;
					if (response.ContentEncoding.ToLower().Contains("gzip"))
					{
						streamToRead = new GZipStream(streamToRead, CompressionMode.Decompress);
					}
					else if (response.ContentEncoding.ToLower().Contains("deflate"))
					{
						streamToRead = new DeflateStream(streamToRead, CompressionMode.Decompress);
					}

					using (StreamReader streamReader = new StreamReader(streamToRead, Encoding.UTF8))
					{
						string responseContent = streamReader.ReadToEnd();
						response.Close();

						var jsonSerializerSettings = new JsonSerializerSettings();
						jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
						Root returnFromNasdaq = JsonConvert.DeserializeObject<Root>(responseContent, jsonSerializerSettings);

						if (returnFromNasdaq != null && returnFromNasdaq.data != null && returnFromNasdaq.data.calendar != null && returnFromNasdaq.data.calendar.rows != null)
						{
							foreach (var itemCalendar in returnFromNasdaq.data.calendar.rows)
							{
								dividendCalendarItems.Add(new DividendCalendarItem() { Ticker = itemCalendar.symbol, PaymentDate = itemCalendar.payment_Date, DividendExDate = itemCalendar.dividend_Ex_Date, AnnouncementDate = itemCalendar.announcement_Date, Value = itemCalendar.dividend_Rate.ToString("F6", CultureInfo.InvariantCulture) });
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

