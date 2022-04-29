using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.MercadoBitcoin.Interface.Model
{
	public class Header
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000061 RID: 97 RVA: 0x0000232A File Offset: 0x0000052A
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00002332 File Offset: 0x00000532
		public string TAPI_ID { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000063 RID: 99 RVA: 0x0000233B File Offset: 0x0000053B
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00002343 File Offset: 0x00000543
		public string TAPI_MAC { get; private set; }

		// Token: 0x06000065 RID: 101 RVA: 0x0000234C File Offset: 0x0000054C
		public Header(string TAPI_ID, string TAPI_MAC)
		{
			this.TAPI_ID = TAPI_ID;
			this.TAPI_MAC = TAPI_MAC;
		}
	}
}
