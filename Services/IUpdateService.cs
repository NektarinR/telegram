using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.Services
{
    public interface IUpdateService
    {
        Task SendAnswerAsync(Update update);
    }
}