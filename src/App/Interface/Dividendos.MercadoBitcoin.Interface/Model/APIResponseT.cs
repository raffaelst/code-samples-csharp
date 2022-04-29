using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.MercadoBitcoin.Interface.Model
{
	public class ApiResponse<T> : ApiResponse
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00002BCA File Offset: 0x00000DCA
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00002BD2 File Offset: 0x00000DD2
		public T response_data { get; set; }

		// Token: 0x060000B0 RID: 176 RVA: 0x00002BDB File Offset: 0x00000DDB
		public ApiResponse(string content) : base(content)
		{
		}
	}
}
