using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ether_bot.Models;
using Telegram.Bot.Types;

namespace Ether_bot.Interfaces
{
    public interface IStorageService
    {
        Task<bool> IsExistUserAsync(int idUser);

        Task CreateUserAsync(int idUser);

        Task<UserModel> GetUserAsync(int idUser);

        Task<IEnumerable<CommandsModel>> GetListCommandsAsync(string command);


        Task<CurrencyModel> GetCurrencyAsync(string currency);

        Task<ExchangeModel> GetExchangeAsync(string exchange);

        
        Task SetNewCurrencyAsync(UserModel user, CurrencyModel currency);

        Task SetNewExchangeAsync(UserModel user, ExchangeModel exchange);

        Task<string> GetTextCommandAsync(string command);
    }
}