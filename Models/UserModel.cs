using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ether_bot.Models
{
    public class UserModel
    {
        public int Id {get;set;}
        public DateTime RegistrationDate {get;set;}
        public string Name {get;set;}
        public long IdChat {get;set;}
        public int CurrencyId {get;set;}
        public virtual CurrencyModel Currency {get;set;}
        public int ExchangeId {get;set;}
        public virtual ExchangeModel Exchange {get;set;}
        public int? TimeUpdate {get;set;}

        public int StateModelId {get;set;}
        public virtual StateModel State {get;set;}
    }
}