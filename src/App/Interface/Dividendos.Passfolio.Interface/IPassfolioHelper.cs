using System;
using System.Collections.Generic;
using Dividendos.Passfolio.Interface.Model;
using K.Logger;

namespace Dividendos.Passfolio.Interface
{
	public interface IPassfolioHelper
	{
		string Session(string email, string password);
		AuthenticatorResponse Authenticators(string auth);
		void SendCode(string auth, string authenticatorID);
		bool SessionMFA(string auth, string code, string authenticatorID);
		ImportPassfolioResult Import(string auth, ILogger logger);

		List<Ticker> GetQuotationOfCryptos(string auth);
	}
}
