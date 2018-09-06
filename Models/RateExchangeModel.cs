using System;
using System.Collections.Generic;

namespace Ether_bot.Models
{
    public class RateExchangeModel
    {
        public int IdPairModel {get;set;}
        public int IdExchangeModel {get;set;}
        public decimal Rate {get;set;}
        public DateTime Time {get;set;}
    }
}