using System;
using System.Collections.Generic;

namespace Ether_bot.Models
{
    public class TextModel
    {
        public int Id {get;set;}
        public string Text {get;set;}

        public virtual ICollection<CommandsModel> Commnads {get;set;}
    }
}