using System;

namespace Ether_bot.Models
{
    public class RateExchangeModel
    {
        public int PairId {get;set;}
        public int ExchangeId {get;set;}
        public decimal Rate {get;set;}
        public DateTime Time {get;set;}
    }
}