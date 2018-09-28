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
using Ether_bot.Interfaces;

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
        public async Task<decimal?> GetRateAsync(ExchangeModel exchange)
        {
            decimal result;         
            if (!_cache.TryGetValue((exchange.Pair, exchange.Exchange), out result))   
            {
                string response;
                switch (exchange.Exchange)
                {
                    case "EXMO":
                        response = await _exchangeClient.GetStringAsync(_exchangeSettings.Value.Exmo);
                        //var strResult = await response.Content.ReadAsStringAsync();
                        var dictResult = JsonConvert.DeserializeObject<Dictionary<string,PairExmo>>(response);
                        PairExmo _pair;
                        if (!dictResult.TryGetValue($"{exchange.Pair}", out _pair))
                            return null;
                        result = decimal.Parse(_pair.buy_price);
                    break;
                    case "BINANCE":
                        response = await _exchangeClient.GetStringAsync(_exchangeSettings.Value.Binance);
                        //var strResult = await response.Content.ReadAsStringAsync();
                        var binaJson = JsonConvert.DeserializeObject<BinancePair>(response);                        
                        result = decimal.Parse(binaJson.Price);
                    break;
                }                
                _cache.Set((exchange.Pair, exchange.Exchange), result,
                    new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
                    }); 
            }
            return result;
        }
    }
}