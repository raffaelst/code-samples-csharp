
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.MercadoBitcoin.Interface.Model
{
	public class ApiResponse
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00002B61 File Offset: 0x00000D61
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00002B69 File Offset: 0x00000D69
		public int status_code { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00002B72 File Offset: 0x00000D72
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x00002B7A File Offset: 0x00000D7A
		public string error_message { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00002B83 File Offset: 0x00000D83
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x00002B8B File Offset: 0x00000D8B
		public string server_unix_timestamp { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00002B94 File Offset: 0x00000D94
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00002B9C File Offset: 0x00000D9C
		public string Content { get; private set; }

		// Token: 0x060000AA RID: 170 RVA: 0x00002BA5 File Offset: 0x00000DA5
		public ApiResponse(string content)
		{
			this.SetContent(content);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00002072 File Offset: 0x00000272
		public ApiResponse()
		{
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00002BB4 File Offset: 0x00000DB4
		public void SetContent(string content)
		{
			this.Content = content;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00002BBD File Offset: 0x00000DBD
		//public T GetDeserialize<T>()
		//{
		//	return JsonConvert.DeserializeObject<T>(this.Content);
		//}
	}
}
