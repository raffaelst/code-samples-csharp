using System;
using IO.Swagger.Api;

namespace Dividendos.FunctionalAPITest
{
	public class Tests
	{
		public Tests()
		{
			AuthApi authApi = new AuthApi();
			authApi.AuthLoginPost(new IO.Swagger.Model.LoginModel() { GrantType = "password", Password = "123456", Username = "henri@gmail.com" }, "1");
		}
	}
}

