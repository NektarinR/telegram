using System;
using Telegram.Bot;

namespace Ether_bot.Services
{
    public interface IBotService
    {
        TelegramBotClient TlgBotClient{get;}   
    }
}