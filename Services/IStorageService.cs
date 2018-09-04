using System;
using System.Threading.Tasks;
using Ether_bot.Models;
using Telegram.Bot.Types;

namespace Ether_bot.Services
{
    public interface IStorageService
    {
        Task CreateUserAsync(int idUser, string name, DateTime regTime, 
            long idChat, string state, string currency = "USD", string exchange ="exmo.me", 
            int? timeUpdate = null);
        Task<(string currency,string exchange,string timeUpdate)> GetSettingsUserAsync(int idUser);
        
        Task UpdateSettingsUserAsync(int idUser, string newCurrency,string newExchange, int timeUpdate);
        Task<StateModel> GetUserStateAsync(int idUser);
        Task UpdateUserStateAsync(int idUser, string newState, DateTime changeTime, int idMessage);
    }
}