using Dividendos.MercadoBitcoin.Interface;
using Dividendos.MercadoBitcoin.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.MercadoBitcoin
{
	public class AccountInfoClient : IAccountInfo
	{
		private IBaseClient _baseClient;

		public AccountInfoClient()
		{
	
		}

	
		public async Task<ApiResponse<AccountInfo>> GetAsync(RequestHeader requestHeader)
		{
			this._baseClient = new BaseClient(requestHeader);
			return await this._baseClient.GetResponseAsync<AccountInfo>(new ApiRequest("get_account_info"));
		}
		
	}
}
