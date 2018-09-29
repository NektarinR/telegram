using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ether_bot.Interfaces;
using Ether_bot.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.Commands
{
    public class CallbackKeyboard : IKeyboard
    {
        public async Task<InlineKeyboardMarkup> GetKeyboardAsync(StateModel state, IStorageService storageService)
        {
            var commnds = await storageService.GetListCommands(state.State);
            List<List<InlineKeyboardButton>> lstBtns = new List<List<InlineKeyboardButton>>();
            int i = 0;
            int k = 0;
            lstBtns.Add(new List<InlineKeyboardButton>());
            foreach (var tmpBtn in commnds)
            {
                if (i == 2){
                    lstBtns.Add(new List<InlineKeyboardButton>());
                    i = 0; k++;
                }
                lstBtns[k].Add( new InlineKeyboardButton(){
                    CallbackData = tmpBtn.Command,
                    Text = tmpBtn.Command
                });
                i++;
            }
            var result = new InlineKeyboardMarkup(lstBtns);
            return new InlineKeyboardMarkup(lstBtns);
        }
    }
}