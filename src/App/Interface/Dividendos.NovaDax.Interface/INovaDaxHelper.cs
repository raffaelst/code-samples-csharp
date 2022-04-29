using Dividendos.NovaDax.Interface;
using Dividendos.NovaDax.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.NovaDax
{
	public interface INovaDaxHelper
	{
		Root GetUserPosition(string apiKey, string apiSecret);
	}
}

