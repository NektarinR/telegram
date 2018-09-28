using System;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.Interfaces
{
    public interface IBotService
    {
        TelegramBotClient TlgBotClient{get;}        
    }
}