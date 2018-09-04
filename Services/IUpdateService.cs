using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Ether_bot.Services
{
    public interface IUpdateService
    {
        Task SendAnswerAsync(Update update);
    }
}