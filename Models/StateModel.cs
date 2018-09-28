using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ether_bot.Models
{
    public class StateModel
    {
        public int Id {get;set;}
        public string State{get;set;}
        public virtual ICollection<UserModel> Users {get;set;}
        public virtual ICollection<CommandsModel> Commands {get;set;}
    }
}