using System;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.Services
{
    public interface IBotService
    {
        TelegramBotClient TlgBotClient{get;}        
        ReplyKeyboardMarkup GetKeyboardByState(States state);
    }
}