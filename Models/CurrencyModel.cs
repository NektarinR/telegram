using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ether_bot.Models
{
    public class CurrencyModel
    {
        public int Id {get;set;}
        [StringLength(30)]
        public string Currency {get;set;}
        public virtual ICollection<UserModel> Users {get;set;}

    }
}