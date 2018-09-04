using System;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.Services
{
    public interface IUserState
    {
        ReplyKeyboardMarkup Execute();
        bool CanExecute(Commands command);
    }
}