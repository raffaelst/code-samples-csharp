using Binance.NetCore;
using Dividendos.Binance.Interface;
using Dividendos.Binance.Interface.Model;
using K.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace Dividendos.Binance
{
	public class BinanceHelper : IBinanceHelper
	{
		public List<BalanceResponse> GetUserPosition(string apiKey, string apiSecret, ILogger logger)
		{
			List<BalanceResponse> balances = new List<BalanceResponse>();

			try
            {
				var binance = new BinanceApiClient(apiKey, apiSecret);
				var balance = binance.GetBalance();

				if (balance != null && balance.balances != null)
				{
					foreach (var item in balance.balances)
					{
						if(item.free > 0 && !balances.Exists(balanceItem => balanceItem.asset.Equals(item.asset)))
						{
							balances.Add(new BalanceResponse() { asset = item.asset, free = (item.free + item.locked), locked = item.locked  });
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


		public List<Ticker> GetQuotationOfCryptos()
		{
			List<Ticker> ticker = new List<Ticker>();

			var binance = new BinanceApiClient();
			var ticks = binance.Get24HourStats();

            foreach (var item in ticks)
            {
				if (item.symbol.Contains("USDT"))
				{
					var name = item.symbol.Replace("USDT", "");

					if (!ticker.Exists(itemTicker => itemTicker.name.Equals(name)))
					{
						ticker.Add(new Ticker() { name = name, open = name.Equals("BRL") ? 1 : decimal.Parse(item.openPrice), last = name.Equals("BRL") ? 1 : decimal.Parse(item.lastPrice) });
					}
				}
				else
				{
					if (item.symbol.Contains("BUSD"))
					{
						var name = item.symbol.Replace("BUSD", "");

						if (!ticker.Exists(itemTicker => itemTicker.name.Equals(name)))
						{
							ticker.Add(new Ticker() { name = name, open = decimal.Parse(item.openPrice), last = decimal.Parse(item.lastPrice) });
						}
					}
				}
			}

			return ticker;
		}
	}
}

