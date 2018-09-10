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
        private HttpClient _exchangeClient;
        private IMemoryCache _cache;
        private IOptions<ExchangeSettings> _exchangeSettings;

        public ExchangeService(IOptions<ExchangeSettings> exchangeSettings, IMemoryCache cache)
        {
            _exchangeClient = new HttpClient();
            _exchangeClient.Timeout = new TimeSpan(0,0,10);
            _exchangeSettings = exchangeSettings;
            _cache = cache;
        }
        //Если в словари уже есть пара валюты и биржа валюты, то достаем из словаря
        public async Task<decimal?> GetRateAsync(string pair, string exchange)
        {
            decimal result;         
            if (!_cache.TryGetValue((pair,exchange), out result))   
            {
                string response;
                switch (exchange)
                {
                    case "Exmo":
                        response = await _exchangeClient.GetStringAsync(_exchangeSettings.Value.Exmo);
                        //var strResult = await response.Content.ReadAsStringAsync();
                        var dictResult = JsonConvert.DeserializeObject<Dictionary<string,PairExmo>>(response);
                        PairExmo _pair;
                        if (!dictResult.TryGetValue($"ETH_{pair.ToUpper()}", out _pair))
                            return null;
                        result = decimal.Parse(_pair.buy_price);
                    break;
                    case "Binance":
                        response = await _exchangeClient.GetStringAsync(_exchangeSettings.Value.Binance);
                        //var strResult = await response.Content.ReadAsStringAsync();
                        var binaJson = JsonConvert.DeserializeObject<BinancePair>(response);                        
                        result = decimal.Parse(binaJson.Price);
                    break;
                }                
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