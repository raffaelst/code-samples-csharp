using Dividendos.Biscoint.Interface;
using Dividendos.Biscoint.Interface.Model;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Biscoint
{
	public interface IBiscointHelper
	{
		Root GetUserPosition(string apiKey, string apiSecret, ILogger logger);
	}
}

