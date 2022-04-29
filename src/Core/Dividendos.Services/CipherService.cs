using Dividendos.Service.Interface;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service
{
    public class CipherService : BaseService, ICipherService
    {
        private readonly IDataProtector _dataProtector;
        private readonly string _key = "dividendos.me.key";
        public CipherService(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtector = dataProtectionProvider.CreateProtector(nameof(CipherService)); ;
        }

        public string Encrypt(string input)
        {
            //var protector = _dataProtector.CreateProtector(_key);
            //return protector.Protect(input);

            return input;
        }

        public string Decrypt(string cipherText)
        {
            //var protector = _dataProtector.CreateProtector(_key);
            //return protector.Unprotect(cipherText);

            return cipherText;
        }
    }
}
