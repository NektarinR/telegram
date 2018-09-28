using System;
using System.Collections.Generic;

namespace Ether_bot.Models
{
    public class CommandsModel
    {
        public int Id {get;set;}
        public string Command {get;set;}
        public int TextId {get;set;}
        public virtual TextModel Text {get;set;}
        
        public int StateId {get;set;}
        public virtual StateModel State {get;set;}

    }
}