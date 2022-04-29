using Dividendos.MercadoBitcoin.Interface;
using Dividendos.MercadoBitcoin.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Dividendos.MercadoBitcoin.Interface
{
	public class RequestHeader
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002362 File Offset: 0x00000562
		// (set) Token: 0x06000067 RID: 103 RVA: 0x0000236A File Offset: 0x0000056A
		public string Id { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002373 File Offset: 0x00000573
		// (set) Token: 0x06000069 RID: 105 RVA: 0x0000237B File Offset: 0x0000057B
		public string Secret { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00002384 File Offset: 0x00000584
		// (set) Token: 0x0600006B RID: 107 RVA: 0x0000238C File Offset: 0x0000058C
		public Uri BaseUrl { get; private set; }

		// Token: 0x0600006C RID: 108 RVA: 0x00002395 File Offset: 0x00000595
		public RequestHeader(string id, string secret)
		{
			this.Id = id;
			this.Secret = secret;
			this.BaseUrl = new Uri("https://www.mercadobitcoin.net/tapi/v3/");
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000023B8 File Offset: 0x000005B8
		public Header GetHeader(ApiRequest apiRequest)
		{
			string message = "/tapi/v3/?" + this.ToStringParam(apiRequest.RequestParams);
			string id = this.Id;
			string tapi_MAC = this.HashString(message, this.Secret);
			return new Header(id, tapi_MAC);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000023F6 File Offset: 0x000005F6
		private string ToStringParam(Dictionary<string, string> param)
		{
			return string.Join("&", (from x in param
									 select x.Key + "=" + x.Value).ToArray<string>());
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000242C File Offset: 0x0000062C
		private string HashString(string message, string key)
		{
			Encoding utf = Encoding.UTF8;
			string result;
			using (HMACSHA512 hmacsha = new HMACSHA512(utf.GetBytes(key)))
			{
				hmacsha.ComputeHash(utf.GetBytes(message));
				result = this.ByteToString(hmacsha.Hash);
			}
			return result;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002484 File Offset: 0x00000684
		private string ByteToString(byte[] buff)
		{
			string text = "";
			for (int i = 0; i < buff.Length; i++)
			{
				text += buff[i].ToString("X2");
			}
			return text.ToLower();
		}
	}
}
