using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.MercadoBitcoin.Interface.Model
{
	public class Currency
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000020D7 File Offset: 0x000002D7
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000020DF File Offset: 0x000002DF
		public string available { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000020E8 File Offset: 0x000002E8
		// (set) Token: 0x06000015 RID: 21 RVA: 0x000020F0 File Offset: 0x000002F0
		public string total { get; set; }

		// Token: 0x06000016 RID: 22 RVA: 0x00002072 File Offset: 0x00000272
		public Currency()
		{
		}
	}
}
