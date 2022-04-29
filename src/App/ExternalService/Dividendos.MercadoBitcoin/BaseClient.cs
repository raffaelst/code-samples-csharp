
using Dividendos.MercadoBitcoin.Interface;
using Dividendos.MercadoBitcoin.Interface.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dividendos.MercadoBitcoin
{
	public class BaseClient : IBaseClient
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002519 File Offset: 0x00000719
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00002521 File Offset: 0x00000721
		public long Nonce { get; private set; }

		// Token: 0x06000076 RID: 118 RVA: 0x0000252A File Offset: 0x0000072A
		public BaseClient(RequestHeader requestHeader)
		{
			this._httpClient = new HttpClient();
			this._requestHeader = requestHeader;
			this.Nonce = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds + 10000000000;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000254C File Offset: 0x0000074C
		public async Task<ApiResponse<TResponse>> GetResponseAsync<TResponse>(ApiRequest apiRequest)
		{
			long nonce = this.Nonce;
			this.Nonce = nonce + 1L;
			this.UpdateRequestParams(apiRequest);
			FormUrlEncodedContent content = this.BuildContent(apiRequest);
			this.SetHttpRequestHeaders(apiRequest);
			string text = await (await this._httpClient.PostAsync(this._requestHeader.BaseUrl, content)).Content.ReadAsStringAsync().ConfigureAwait(false);
			ApiResponse<TResponse> apiResponse = this.Deserialize<ApiResponse<TResponse>>(text);
			apiResponse.SetContent(text);
			return await this.ResultManage<TResponse>(apiRequest, apiResponse);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002599 File Offset: 0x00000799
		public void SetNonce(long nonce)
		{
			this.Nonce = nonce + 1L;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000025A8 File Offset: 0x000007A8
		protected void UpdateRequestParams(ApiRequest apiRequest)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{
				{
					"tapi_method",
					apiRequest.Method
				},
				{
					"tapi_nonce",
					this.Nonce.ToString()
				}
			};
			foreach (KeyValuePair<string, string> keyValuePair in apiRequest.RequestParams)
			{
				if (keyValuePair.Key != "tapi_method" && keyValuePair.Key != "tapi_nonce")
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			apiRequest.UpdateParam(dictionary);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002668 File Offset: 0x00000868
		private FormUrlEncodedContent BuildContent(ApiRequest apiRequest)
		{
			return new FormUrlEncodedContent(apiRequest.RequestParams);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002678 File Offset: 0x00000878
		private void SetHttpRequestHeaders(ApiRequest apiRequest)
		{
			Header header = this._requestHeader.GetHeader(apiRequest);
			if (this._httpClient.DefaultRequestHeaders.Any((KeyValuePair<string, IEnumerable<string>> x) => x.Key == "TAPI-ID"))
			{
				this._httpClient.DefaultRequestHeaders.Remove("TAPI-ID");
			}
			if (this._httpClient.DefaultRequestHeaders.Any((KeyValuePair<string, IEnumerable<string>> x) => x.Key == "TAPI-MAC"))
			{
				this._httpClient.DefaultRequestHeaders.Remove("TAPI-MAC");
			}
			this._httpClient.DefaultRequestHeaders.Add("TAPI-ID", header.TAPI_ID);
			this._httpClient.DefaultRequestHeaders.Add("TAPI-MAC", header.TAPI_MAC);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002756 File Offset: 0x00000956
		private T Deserialize<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002760 File Offset: 0x00000960
		private async Task<ApiResponse<T>> ResultManage<T>(ApiRequest apiRequest, ApiResponse<T> apiResponse)
		{
			int status_code = apiResponse.status_code;
			ApiResponse<T> result;
			if (status_code == 203)
			{
				this.SetNonce(Convert.ToInt64(apiResponse.error_message.Split(new char[]
				{
					':'
				})[1].Replace(".", "")));
				result = await this.GetResponseAsync<T>(apiRequest);
			}
			else
			{
				result = apiResponse;
			}
			return result;
		}

		// Token: 0x04000035 RID: 53
		private readonly HttpClient _httpClient;

		// Token: 0x04000036 RID: 54
		private readonly RequestHeader _requestHeader;
	}
}
