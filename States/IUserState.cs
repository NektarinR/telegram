using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.States
{
    public interface IUserState
    {        
        Task GoToSettingsAsync();
        Task GoToCurrencyAsync();
        Task GoToExchangeAsync();
        Task GoToTimeNotifyAsync();
        Task ReturnAsync();
    }
}