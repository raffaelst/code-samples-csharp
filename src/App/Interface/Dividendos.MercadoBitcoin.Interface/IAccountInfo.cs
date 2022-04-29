using Dividendos.MercadoBitcoin.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.MercadoBitcoin.Interface
{
	public interface IAccountInfo
	{
		// Token: 0x06000091 RID: 145
		Task<ApiResponse<AccountInfo>> GetAsync(RequestHeader requestHeader);
	}
}
