using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ether_bot.Models
{
    public class StateModel
    {
        public int Id {get;set;}
        [StringLength(50)]
        public string State{get;set;}
        public DateTime ChangeDate{get;set;}
        public long IdMessage{get;set;}

        public virtual UserModel User {get;set;}
    }
}