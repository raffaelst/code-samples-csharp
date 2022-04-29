using Dividendos.CrossCutting.Config.Model;
using Dividendos.Service.Interface;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service
{
    public class CacheService : BaseService, ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IOptions<GlobalSystemConfig> _globalSystemConfig;
        private readonly K.Logger.ILogger _logger;

        public CacheService(IDistributedCache cache,
            IOptions<GlobalSystemConfig> globalSystemConfig,
            K.Logger.ILogger logger)
        {
            _cache = cache;
            _globalSystemConfig = globalSystemConfig;
            _logger = logger;
        }

        public string GetFromCache(string key)
        {
            string valueFromCache = null;

            if (_globalSystemConfig.Value.UseCacheService)
            {
                try
                {
                    valueFromCache = _cache.GetString(key);
                }
                catch (Exception ex)
                {
                    _logger.SendErrorAsync(ex);
                }
            }
            return valueFromCache;
        }

        public void SaveOnCache(string key, TimeSpan timeToExpiration, string data)
        {
            if (_globalSystemConfig.Value.UseCacheService)
            {

                DistributedCacheEntryOptions opcoesCache =
                               new DistributedCacheEntryOptions();
                opcoesCache.SetAbsoluteExpiration(timeToExpiration);

                try
                {
                    _cache.SetString(key, data, opcoesCache);
                }
                catch (Exception ex)
                {
                    _logger.SendErrorAsync(ex);
                }
            }
        }


        public void DeleteOnCache(string key)
        {
            if (_globalSystemConfig.Value.UseCacheService)
            {
                try
                {
                    _cache.Remove(key);
                }
                catch (Exception ex)
                {
                    _logger.SendErrorAsync(ex);
                }
            }
        }
    }
}
