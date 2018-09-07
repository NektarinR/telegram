using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Ether_bot.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace Ether_bot.Services
{
    public class ExchangeService : IExchangeService
    {
        private readonly HttpClient _exchangeClient;
        private ConcurrentDictionary<(string pairKey,string exchangeKey),RatePair> cacheRate;
        private IMemoryCache _cache;

        public ExchangeService(IOptions<ExchangeSettings> exchangeSettings, IMemoryCache cache)
        {
            _exchangeClient = new HttpClient();
            _exchangeClient.Timeout = new TimeSpan(0,0,10);
            _exchangeClient.BaseAddress = new Uri(exchangeSettings.Value.ExmoApi);
            _cache = cache;
            cacheRate = new ConcurrentDictionary<(string pairKey, string exchangeKey), RatePair>(Environment.ProcessorCount*2,30);
        }
        //Если в словари уже есть пара валюты и биржа валюты, то достаем из словаря
        public async Task<decimal?> GetRateAsync(string pair, string exchange)
        {
            decimal result;         
            if (!_cache.TryGetValue((pair,exchange), out result))   
            {
                var response = await _exchangeClient.GetAsync(@"v1/ticker");
                var strResult = await response.Content.ReadAsStringAsync();
                var dictResult = JsonConvert.DeserializeObject<Dictionary<string,Pair>>(strResult);
                Pair _pair;
                if (!dictResult.TryGetValue(pair, out _pair))
                    return null;
                result = decimal.Parse(_pair.buy_price);
                _cache.Set((pair,exchange), result,
                    new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
                    }); 
            }
            return decimal.Round(result,2);
        }
    }
}