using System;
using System.Threading.Tasks;
using Ether_bot.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.Interfaces
{
    interface IKeyboard
    {
        Task<InlineKeyboardMarkup> GetKeyboardAsync(string command, IStorageService storageService);
    }
}