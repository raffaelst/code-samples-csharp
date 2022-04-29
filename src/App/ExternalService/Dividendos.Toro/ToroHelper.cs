using Dividendos.Toro.Interface;
using Dividendos.Toro.Interface.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dividendos.Toro
{
	public class ToroHelper : IToroHelper
	{
		public ImportToroResult ValidateUser(string user, string password, string token = null)
		{
			ImportToroResult importAvenueResult = new ImportToroResult();

			var client = new RestClient("https://webapieqr.toroinvestimentos.com.br/auth/authentication/login");
			client.Timeout = -1;
			var request2 = new RestRequest(Method.POST);
			request2.AddHeader("Content-Type", "text/plain");
			var body = string.Format(@"username={0}&password={1}&client_id=Hub&grant_type=password", user, password);

			if (!string.IsNullOrWhiteSpace(token))
			{
				body = body + "&X-TOKEN_TYPE=TokenTime&X-TOKEN_CATEGORY=Simple&X-TOKEN=" + token;
			}


			request2.AddParameter("text/plain", body, ParameterType.RequestBody);
			IRestResponse response = client.Execute(request2);

			if (response.IsSuccessful)
			{
				dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(response.Content);

				importAvenueResult.Success = true;
				importAvenueResult.JwtToken = resultAPI.access_token;
				importAvenueResult.ApiResult = response.Content;
			}
			else
			{
				dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(response.Content);
				importAvenueResult.ApiResult = response.Content;

				if (resultAPI.error != null && (resultAPI.error == "second_factor_required" || (resultAPI.error == "captcha_required")))
				{
					importAvenueResult.Success = true;
					string sessionId = GetSessionId();
					bool success = SendSms(sessionId, user);
				}
			}

			return importAvenueResult;
		}

		public ImportToroResult ImportToro(string user, string password, DateTime? lastEventDate, string token = null)
		{
			ImportToroResult importToroResult = ValidateUser(user, password, token);

			if (importToroResult.Success && !string.IsNullOrWhiteSpace(importToroResult.JwtToken))
			{
				List<DateTime> dates = GetTransactionDates(importToroResult.JwtToken, importToroResult);

				if (dates != null && dates.Count() > 0)
				{
					if (lastEventDate.HasValue)
					{
						dates = dates.Where(dt => dt >= lastEventDate.Value.Date).ToList();
					}

					if (dates != null && dates.Count() > 0)
					{
						List<ToroTransactionItem> toroTransactionItems = new List<ToroTransactionItem>();
						List<Value> toroPlannedTransactions = new List<Value>();
						importToroResult.Orders = new List<ToroOrder>();

						foreach (DateTime date in dates)
						{
							toroTransactionItems.AddRange(GetToroTransactions(importToroResult.JwtToken, date, importToroResult));
							toroPlannedTransactions.AddRange(GetPlannedTransactions(importToroResult.JwtToken, date, importToroResult));
						}

						if (toroTransactionItems != null && toroTransactionItems.Count() > 0)
						{
							toroTransactionItems = toroTransactionItems.Where(toroT => toroT.Status == "50" && (toroT.SideDescription == "Sell" || toroT.SideDescription == "Buy")).ToList();

							if (toroTransactionItems != null && toroTransactionItems.Count() > 0)
							{
								foreach (ToroTransactionItem toroTransactionItem in toroTransactionItems)
								{
									if (toroTransactionItem == null || string.IsNullOrWhiteSpace(toroTransactionItem.ExecutionDate))
                                    {
										continue;
                                    }

									int year = Convert.ToInt32(toroTransactionItem.ExecutionDate.Substring(0, 4));
									int month = Convert.ToInt32(toroTransactionItem.ExecutionDate.Substring(5, 2));
									int day = Convert.ToInt32(toroTransactionItem.ExecutionDate.Substring(8, 2));
									int hour = Convert.ToInt32(toroTransactionItem.ExecutionDate.Substring(11, 2));
									int minutes = Convert.ToInt32(toroTransactionItem.ExecutionDate.Substring(14, 2));
									int seconds = Convert.ToInt32(toroTransactionItem.ExecutionDate.Substring(17, 2));

									toroTransactionItem.EventDate = new DateTime(year, month, day, hour, minutes, seconds);
								}

								toroTransactionItems = toroTransactionItems.OrderBy(toroT => toroT.EventDate).ToList();

								foreach (ToroTransactionItem toroTransactionItem in toroTransactionItems)
								{
									decimal avgPrice = 0;
									decimal.TryParse(toroTransactionItem.AvaragePriceExecuted, NumberStyles.Currency, CultureInfo.InvariantCulture, out avgPrice);

									decimal quantity = 0;
									decimal.TryParse(toroTransactionItem.OrderQty, NumberStyles.Currency, CultureInfo.InvariantCulture, out quantity);

									ToroOrder toroOrder = new ToroOrder();
									toroOrder.AveragePrice = avgPrice;
									toroOrder.EventDate = toroTransactionItem.EventDate;
									toroOrder.IdOperationType = toroTransactionItem.SideDescription == "Buy" ? 1 : 2;
									toroOrder.NumberOfShares = quantity;
									toroOrder.Symbol = RemoveFractionalLetter(toroTransactionItem.Symbol);
									toroOrder.TransactionId = toroTransactionItem.ClOrderId;

									importToroResult.Orders.Add(toroOrder);
								}
							}
						}

						if (toroPlannedTransactions != null && toroPlannedTransactions.Count > 0)
						{
							toroPlannedTransactions = toroPlannedTransactions.Where(toroT => !string.IsNullOrWhiteSpace(toroT.ExecutionDate)).ToList();

							if (toroPlannedTransactions != null && toroPlannedTransactions.Count > 0)
							{
								foreach (Value value in toroPlannedTransactions)
								{
									int year = Convert.ToInt32(value.ExecutionDate.Substring(0, 4));
									int month = Convert.ToInt32(value.ExecutionDate.Substring(5, 2));
									int day = Convert.ToInt32(value.ExecutionDate.Substring(8, 2));
									int hour = Convert.ToInt32(value.ExecutionDate.Substring(11, 2));
									int minutes = Convert.ToInt32(value.ExecutionDate.Substring(14, 2));
									int seconds = Convert.ToInt32(value.ExecutionDate.Substring(17, 2));

									DateTime eventDate = new DateTime(year, month, day, hour, minutes, seconds);

									if (value.Legs != null && value.Legs.Count > 0)
									{
										List<Leg> legs = value.Legs.Where(toroT => toroT.Status == "50" && (toroT.SideDescription == "Sell" || toroT.SideDescription == "Buy")).ToList();

										if (legs != null && legs.Count > 0)
										{
											foreach (Leg leg in legs)
											{
												decimal avgPrice = 0;
												decimal.TryParse(leg.AvaragePriceExecuted, NumberStyles.Currency, CultureInfo.InvariantCulture, out avgPrice);

												decimal quantity = 0;
												decimal.TryParse(leg.OrderQty, NumberStyles.Currency, CultureInfo.InvariantCulture, out quantity);

												ToroOrder toroOrder = new ToroOrder();
												toroOrder.AveragePrice = avgPrice;
												toroOrder.EventDate = eventDate;
												toroOrder.IdOperationType = leg.SideDescription == "Buy" ? 1 : 2;
												toroOrder.NumberOfShares = quantity;
												toroOrder.Symbol = RemoveFractionalLetter(leg.Symbol);
												toroOrder.TransactionId = leg.ClOrderId;

												importToroResult.Orders.Add(toroOrder);
											}
										}
									}
								}
							}
						}
					
					}
				}

				ToroBondApi toroBondApi = GetToroBonds(importToroResult.JwtToken, importToroResult);

				if (toroBondApi != null && toroBondApi.Bonds != null && toroBondApi.Bonds.Count() > 0)
				{
					importToroResult.Bonds = new List<ToroBond>();

					foreach (Bond bond in toroBondApi.Bonds)
					{
						decimal value = 0;
						decimal.TryParse(bond.PendingAcquireValue, NumberStyles.Currency, CultureInfo.InvariantCulture, out value);

						decimal currentValue = 0;
						decimal.TryParse(bond.CurrentValue, NumberStyles.Currency, CultureInfo.InvariantCulture, out currentValue);

						ToroBond toroBond = new ToroBond();
						toroBond.Name = bond.BondDescription;
						toroBond.Issuer = bond.IssuerName;

						if (value > 0)
						{
							toroBond.Value = value;
						}
						else if (currentValue > 0)
						{
							toroBond.Value = currentValue;
						}


						importToroResult.Bonds.Add(toroBond);
					}
				}

				List<ToroFundApi> toroFundApis = GetToroFunds(importToroResult.JwtToken, importToroResult);

				if (toroFundApis != null && toroFundApis.Count() > 0)
				{
					importToroResult.Funds = new List<ToroFund>();

					foreach (ToroFundApi fundApi in toroFundApis)
					{
						if (fundApi.Fund != null)
						{
							decimal value = 0;
							decimal.TryParse(fundApi.Amount, NumberStyles.Currency, CultureInfo.InvariantCulture, out value);

							ToroFund toroFund = new ToroFund();
							toroFund.Name = fundApi.Fund.Name;
							toroFund.Value = value;

							importToroResult.Funds.Add(toroFund);
						}
					}
				}

			}

			return importToroResult;
		}

		private static string RemoveFractionalLetter(string stock)
		{
			if (!string.IsNullOrEmpty(stock) && (stock[stock.Length - 1].ToString().ToUpper() == "F"))
			{
				stock = stock.Remove(stock.Length - 1);
			}

			return stock;
		}

		private string GetSessionId()
		{
			string sessionId = string.Empty;

			var client = new RestClient("https://webapieqr.toroinvestimentos.com.br/auth/authentication/session/Hub");
			client.Timeout = -1;
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);

			if (response.IsSuccessful)
			{
				dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(response.Content);

				if (resultAPI.value != null)
				{
					sessionId = resultAPI.value;
				}
			}

			return sessionId;
		}

		private bool SendSms(string sessionId, string user)
		{
			bool success = false;
			var client = new RestClient("https://webapieqr.toroinvestimentos.com.br/auth/authentication/token/request");
			client.Timeout = -1;
			var request = new RestRequest(Method.POST);
			request.AddHeader("x-sessionid", sessionId);
			request.AddHeader("Content-Type", "application/json");
			var body = @"{""type"":1,""username"":""" + user + @""",""clientId"":""Hub""}";
			request.AddParameter("application/json", body, ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);

			if (response.IsSuccessful)
			{
				success = true;
			}

			return success;
		}

		private List<DateTime> GetTransactionDates(string jwtToken, ImportToroResult importToroResult)
		{
			List<DateTime> dates = new List<DateTime>();
			var client = new RestClient("https://webapieqr.toroinvestimentos.com.br/exchange/tradingdates");
			client.Timeout = -1;
			var request = new RestRequest(Method.GET);
			request.AddHeader("authorization", "Bearer " + jwtToken);
			IRestResponse response = client.Execute(request);

			importToroResult.ApiResult = response.Content;

			if (response.IsSuccessful)
			{
				var jsonSerializerSettings = new JsonSerializerSettings();
				jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
				jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
				dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(response.Content, jsonSerializerSettings);

				if (resultAPI.value != null)
				{
					foreach (object date in resultAPI.value)
					{
						dates.Add(Convert.ToDateTime(date));
					}

					//dates = (List<DateTime>)resultAPI.value;

					if (dates != null && dates.Count() > 0)
					{
						dates = dates.OrderBy(dt => dt).ToList();
					}

					//foreach (string date in resultAPI.value)
					//{
					//	dates.Add(DateTime.ParseExact(date, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
					//}
				}
			}

			return dates;
		}

		private List<ToroTransactionItem> GetToroTransactions(string jwtToken, DateTime orderDate, ImportToroResult importToroResult)
		{
			List<ToroTransactionItem> toroTransactionItems = new List<ToroTransactionItem>();
			string url = string.Format(@"https://webapieqr.toroinvestimentos.com.br/exchange/order/client?$filter=((ExecutionDate ne null) and (day(ExecutionDate) eq {0} and month(ExecutionDate) eq {1} and year(ExecutionDate) eq {2})) or ((CreationDate ne null) and (day(CreationDate) eq {0} and month(CreationDate) eq {1} and year(CreationDate) eq {2}))&$orderby=CreationDate,TransactionTime", orderDate.Day, orderDate.Month, orderDate.Year);
			var client = new RestClient(url);
			client.Timeout = -1;
			var request = new RestRequest(Method.GET);
			request.AddHeader("authorization", "Bearer " + jwtToken);
			IRestResponse response = client.Execute(request);

			if (response.IsSuccessful)
			{
				var jsonSerializerSettings = new JsonSerializerSettings();
				jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
				jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
				ToroTransaction toroTransaction = JsonConvert.DeserializeObject<ToroTransaction>(response.Content, jsonSerializerSettings);

				if (toroTransaction != null && toroTransaction.ToroTransactionItems != null && toroTransaction.ToroTransactionItems.Count() > 0)
				{
					toroTransactionItems = toroTransaction.ToroTransactionItems;
				}
			}

			if (string.IsNullOrWhiteSpace(importToroResult.ApiResult))
			{
				importToroResult.ApiResult = response.Content;
			}
			else
			{
				importToroResult.ApiResult = importToroResult.ApiResult + response.Content;
			}

			return toroTransactionItems;
		}

		private ToroBondApi GetToroBonds(string jwtToken, ImportToroResult importToroResult)
		{
			ToroBondApi toroBond = null;

			var client = new RestClient("https://webapieqr.toroinvestimentos.com.br/fixedincome/bondsposition");
			client.Timeout = -1;
			var request = new RestRequest(Method.GET);
			request.AddHeader("authorization", "Bearer " + jwtToken);
			IRestResponse response = client.Execute(request);

			if (response.IsSuccessful)
			{
				var jsonSerializerSettings = new JsonSerializerSettings();
				jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
				jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
				toroBond = JsonConvert.DeserializeObject<ToroBondApi>(response.Content, jsonSerializerSettings);
			}

			if (string.IsNullOrWhiteSpace(importToroResult.ApiResult))
			{
				importToroResult.ApiResult = response.Content;
			}
			else
			{
				importToroResult.ApiResult = importToroResult.ApiResult + response.Content;
			}

			return toroBond;
		}

		private List<ToroFundApi> GetToroFunds(string jwtToken, ImportToroResult importToroResult)
		{
			List<ToroFundApi> toroFunds = new List<ToroFundApi>();
			var client = new RestClient("https://jn5b07qfzf.execute-api.sa-east-1.amazonaws.com/prd/positions/consolidated");
			client.Timeout = -1;
			var request = new RestRequest(Method.GET);
			request.AddHeader("authorization", "Bearer " + jwtToken);
			IRestResponse response = client.Execute(request);

			if (response.IsSuccessful)
			{
				var jsonSerializerSettings = new JsonSerializerSettings();
				jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
				jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
				toroFunds = JsonConvert.DeserializeObject<List<ToroFundApi>>(response.Content, jsonSerializerSettings);
			}

			if (string.IsNullOrWhiteSpace(importToroResult.ApiResult))
			{
				importToroResult.ApiResult = response.Content;
			}
			else
			{
				importToroResult.ApiResult = importToroResult.ApiResult + response.Content;
			}

			return toroFunds;
		}

		private List<Value> GetPlannedTransactions(string jwtToken, DateTime orderDate, ImportToroResult importToroResult)
        {
			List<Value> toroPlanTransactions = new List<Value>();
			string url = string.Format("https://webapieqr.toroinvestimentos.com.br/exchange/strategy/client?$filter=((ExecutionDate ne null) and (day(ExecutionDate) eq {0} and month(ExecutionDate) eq {1} and year(ExecutionDate) eq {2})) or ((CreationDate ne null) and (day(CreationDate) eq {0} and month(CreationDate) eq {1} and year(CreationDate) eq {2}))", orderDate.Day, orderDate.Month, orderDate.Year);
			var client = new RestClient(url);
			client.Timeout = -1;
			var request = new RestRequest(Method.GET);
			request.AddHeader("authorization", "Bearer " + jwtToken);
			var body = @"";
			request.AddParameter("text/plain", body, ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);

			if (response.IsSuccessful)
			{
				var jsonSerializerSettings = new JsonSerializerSettings();
				jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
				jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
				ToroPlanTransaction toroPTransactions = JsonConvert.DeserializeObject<ToroPlanTransaction>(response.Content, jsonSerializerSettings);

				if (toroPTransactions != null && toroPTransactions.Value != null && toroPTransactions.Value.Count > 0)
                {
					toroPlanTransactions = toroPTransactions.Value;
                }
			}

			if (string.IsNullOrWhiteSpace(importToroResult.ApiResult))
			{
				importToroResult.ApiResult = response.Content;
			}
			else
			{
				importToroResult.ApiResult = importToroResult.ApiResult + response.Content;
			}

			return toroPlanTransactions;
		}
	}
}
