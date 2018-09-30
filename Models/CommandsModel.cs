using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ether_bot.Models
{
    public class CommandsModel
    {
        public int Id {get;set;}
        public string Data {get;set;}
        public string Command {get;set;}
        
        public int TextId {get;set;}
        public virtual TextModel Text {get;set;}

        [InverseProperty("ParentCommand")]
        public  virtual ICollection<TreeCommands> Parent {get;set;}
        
        [InverseProperty("Command")]
        public  virtual ICollection<TreeCommands> Child {get;set;}
    }
}