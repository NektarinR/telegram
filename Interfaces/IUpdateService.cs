using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.Interfaces
{
    public interface IUpdateService
    {
        Task SendAnswerAsync(Update update);
    }
}