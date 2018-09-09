using System;
using System.Linq;
using System.Threading.Tasks;
using Ether_bot.Models;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace Ether_bot.Services
{
    public class SqliteStorageService : IStorageService
    {
        
        private readonly EthereumBotContext _ethereumBotContext;

        public SqliteStorageService(EthereumBotContext ethereumBotContext)
        {
            _ethereumBotContext = ethereumBotContext;            
        }

        private async Task<bool> ExistingUserAsync(int idUser)
        {
            return await _ethereumBotContext.Users.FirstOrDefaultAsync(user => user.Id == idUser) != null;        
        }

        public async Task CreateUserAsync(int idUser, string name, DateTime regTime, 
            long idChat, string state, int? timeUpdate = null)
        {
            if (await ExistingUserAsync(idUser))
                return;
            StateModel userState = new StateModel()
            {
                State = state,
                ChangeDate = regTime
            };
            UserModel user = new UserModel()
            {
                Id = idUser,
                IdChat = idChat,
                Name = name,
                RegistrationDate = regTime.ToUniversalTime(),
                Currency = (await _ethereumBotContext.Currencies.FirstOrDefaultAsync(c => c.Currency == "USD")),
                Exchange = (await _ethereumBotContext.Exchanges.FirstOrDefaultAsync(c => c.Exchange == "exmo.me")),
                State = userState,
                TimeUpdate = timeUpdate
            };
            _ethereumBotContext.Users.Add(user);
            await _ethereumBotContext.SaveChangesAsync();
        }

        public async Task<StateModel> GetUserStateAsync(int idUser)
        {
            var stMdl = await _ethereumBotContext.States.FirstOrDefaultAsync(a => a.User.Id == idUser);
            if (stMdl != null)
                return stMdl;
            return null;
        }

        public async Task UpdateUserStateAsync(int idUser, string newState, DateTime changeTime, int idMessage)
        {
            var state = await _ethereumBotContext.States.FirstOrDefaultAsync(st => st.User.Id == idUser);
            state.State = newState;
            state.IdMessage = idMessage;
            state.ChangeDate = changeTime.ToUniversalTime();
            await _ethereumBotContext.SaveChangesAsync();
        }

        public Task UpdateSettingsUserAsync(int idUser, string newCurrency, string newExchange, int timeUpdate)
        {
            throw new NotImplementedException();
        }

        public async Task<(string currency, string exchange, string timeUpdate)> GetSettingsUserAsync(int idUser)
        {
            var user = await _ethereumBotContext.Users.FirstOrDefaultAsync(p => p.Id == idUser);
            return (user.Currency.Currency, user.Exchange.Exchange, 
                user.TimeUpdate.HasValue 
                ? user.TimeUpdate.Value.ToString()
                :"отсутствует");
        }
    }
}