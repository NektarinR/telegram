using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ether_bot.Models
{
    public class TreeCommands
    {
        public int Id {get;set;}
        
        [ForeignKey("ParentCommandId")]
        public int ParentCommandId {get;set;}

        public virtual CommandsModel ParentCommand {get;set;}

        [ForeignKey("CommandId")]
        public int CommandId {get;set;}
        
        public virtual CommandsModel Command {get;set;}
    }
}