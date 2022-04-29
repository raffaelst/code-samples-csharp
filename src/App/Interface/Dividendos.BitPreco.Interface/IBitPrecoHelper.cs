using Dividendos.BitPreco.Interface.Model;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.BitPreco.Interface
{
	public interface IBitPrecoHelper
	{
		Root GetUserPosition(string apiKey, string apiSecret, ILogger logger);
	}
}

