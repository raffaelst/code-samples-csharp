using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.MercadoBitcoin.Interface.Model
{
	public class Balance
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x0000207A File Offset: 0x0000027A
		// (set) Token: 0x06000007 RID: 7 RVA: 0x00002082 File Offset: 0x00000282
		public CryptoCurrency brl { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x0000208B File Offset: 0x0000028B
		// (set) Token: 0x06000009 RID: 9 RVA: 0x00002093 File Offset: 0x00000293
		public CryptoCurrency btc { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10 RVA: 0x0000209C File Offset: 0x0000029C
		// (set) Token: 0x0600000B RID: 11 RVA: 0x000020A4 File Offset: 0x000002A4
		public CryptoCurrency ltc { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000020AD File Offset: 0x000002AD
		// (set) Token: 0x0600000D RID: 13 RVA: 0x000020B5 File Offset: 0x000002B5
		public CryptoCurrency bch { get; set; }


		public CryptoCurrency usdc { get; set; }

		public CryptoCurrency paxg { get; set; }

		public CryptoCurrency link { get; set; }

		public CryptoCurrency wibx { get; set; }

		public CryptoCurrency chz { get; set; }


		public CryptoCurrency eth { get; set; }

		public CryptoCurrency xrp { get; set; }

		public CryptoCurrency uni { get; set; }


		public CryptoCurrency axs { get; set; }
		public CryptoCurrency bat { get; set; }
		public CryptoCurrency crv { get; set; }
		public CryptoCurrency enj { get; set; }
		public CryptoCurrency grt { get; set; }
		public CryptoCurrency knc { get; set; }
		public CryptoCurrency mana { get; set; }
		public CryptoCurrency snx { get; set; }
		public CryptoCurrency zrx { get; set; }
		public CryptoCurrency ygg { get; set; }
		public CryptoCurrency aave { get; set; }

		// Token: 0x0600000E RID: 14 RVA: 0x00002072 File Offset: 0x00000272
		public Balance()
		{
		}
	}
}
