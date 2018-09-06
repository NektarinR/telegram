using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ether_bot.Models
{
    public class ExchangeModel
    {
        public int Id {get;set;}
        public string Exchange {get;set;}
        public virtual ICollection<UserModel> Users {get;set;}
        
    }
}