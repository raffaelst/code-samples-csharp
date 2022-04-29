using Dividendos.MercadoBitcoin.Interface.Model;
using System;
using System.Threading.Tasks;

namespace Dividendos.MercadoBitcoin.Interface
{
    public interface IBaseClient
	{
		// Token: 0x06000092 RID: 146
		Task<ApiResponse<T>> GetResponseAsync<T>(ApiRequest request);

		// Token: 0x06000093 RID: 147
		void SetNonce(long nonce);
	}
}
