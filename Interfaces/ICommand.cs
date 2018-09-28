using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
namespace Ether_bot.Interfaces
{
    public interface ICommand
    {
        Task ExecuteAsync(IBotService botService, Update update);
    }
}