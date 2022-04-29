using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.MercadoBitcoin.Interface.Model
{
	public class CryptoCurrency : Currency
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000020BE File Offset: 0x000002BE
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000020C6 File Offset: 0x000002C6
		public int amount_open_orders { get; set; }

		// Token: 0x06000011 RID: 17 RVA: 0x000020CF File Offset: 0x000002CF
		public CryptoCurrency()
		{
		}
	}
}
