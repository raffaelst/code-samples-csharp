using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.MercadoBitcoin.Interface.Model
{
	public class ApiRequest
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00002B06 File Offset: 0x00000D06
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00002B0E File Offset: 0x00000D0E
		public Dictionary<string, string> RequestParams { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00002B17 File Offset: 0x00000D17
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00002B1F File Offset: 0x00000D1F
		public string Method { get; private set; }

		// Token: 0x0600009F RID: 159 RVA: 0x00002B28 File Offset: 0x00000D28
		public ApiRequest(string method)
		{
			this.Method = method;
			this.RequestParams = new Dictionary<string, string>();
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00002B42 File Offset: 0x00000D42
		public ApiRequest(string method, Dictionary<string, string> requestParams)
		{
			this.Method = method;
			this.RequestParams = requestParams;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00002B58 File Offset: 0x00000D58
		public void UpdateParam(Dictionary<string, string> param)
		{
			this.RequestParams = param;
		}
	}
}
