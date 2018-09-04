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

        public async Task CreateUserAsync(int idUser, string name, DateTime regTime, long idChat, 
            string state, string currency = "USD", string exchange = "exmo.me", int? timeUpdate = null)
        {
            if (await ExistingUserAsync(idUser))
                return;
            StateModel userState = new StateModel()
            {
                State = state,
                ChangeDate = regTime
            };
            _ethereumBotContext.States.Add(userState);
            await _ethereumBotContext.SaveChangesAsync();
            CurrencyModel currMdl = new CurrencyModel()
            {
                Currency = currency
            };
            _ethereumBotContext.Currencies.Add(currMdl);
            await _ethereumBotContext.SaveChangesAsync();
            ExchangeModel exchMdl = new ExchangeModel()
            {
                Exchange = exchange
            };
            _ethereumBotContext.Exchanges.Add(exchMdl);
            await _ethereumBotContext.SaveChangesAsync();
            UserModel user = new UserModel()
            {
                Id = idUser,
                IdChat = idChat,
                Name = name,
                RegistrationDate = regTime,
                CurrencyId = currMdl.Id,
                ExchangeId = exchMdl.Id,
                StateModelId = userState.Id,
                TimeUpdate = timeUpdate
            };
            _ethereumBotContext.Users.Add(user);
            await _ethereumBotContext.SaveChangesAsync();
        }

        public async Task<StateModel> GetUserStateAsync(int idUser)
        {
            var stMdl = await _ethereumBotContext.Users.FirstOrDefaultAsync(a => a.Id == idUser);
            if (stMdl != null)
                return stMdl.State;
            return null;
        }

        public async Task UpdateUserStateAsync(int idUser, string newState, DateTime changeTime, int idMessage)
        {
            var state = await _ethereumBotContext.States.FirstOrDefaultAsync(st => st.User.Id == idUser);
            state.State = newState;
            state.IdMessage = idMessage;
            state.ChangeDate = changeTime;
            await _ethereumBotContext.SaveChangesAsync();
        }

        public Task UpdateSettingsUserAsync(int idUser, string newCurrency, string newExchange, int timeUpdate)
        {
            throw new NotImplementedException();
        }

        public async Task<(string currency, string exchange, string timeUpdate)> GetSettingsUserAsync(int idUser)
        {
            var user = await _ethereumBotContext.Users.FirstOrDefaultAsync(p => p.Id == idUser);
            var tm = user.TimeUpdate.HasValue ?user.TimeUpdate.Value.ToString():"отсутствует";
            return (user.Currency.Currency, user.Exchange.Exchange, 
                user.TimeUpdate.HasValue ? user.TimeUpdate.Value.ToString():"отсутствует");
        }
    }
}