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

        Task<bool> CanExecuteAsync(string state, string cmd);

        Task<IEnumerable<CommandsModel>> GetListCommands(string state);

        Task<StateModel> GetStateAsync(string state);

        Task SetNewState (UserModel user, StateModel state);

        Task<string> GetTextCommand(string command, string state);
    }
}