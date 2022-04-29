using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ICacheService
    {
        void SaveOnCache(string key, TimeSpan timeToExpiration, string data);

        string GetFromCache(string key);

        void DeleteOnCache(string key);
    }
}
