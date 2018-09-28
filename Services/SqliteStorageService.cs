using System;
using System.Linq;
using System.Threading.Tasks;
using Ether_bot.Models;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Ether_bot.Interfaces;
using Ether_bot.Context;
using System.Collections.Generic;

namespace Ether_bot.Services
{
    public class SqliteStorageService : IStorageService
    {
        
        private readonly EthereumBotContext _ethereumBotContext;

        public SqliteStorageService(EthereumBotContext ethereumBotContext)
        {
            _ethereumBotContext = ethereumBotContext;            
        }

        public async Task<bool> CanExecuteAsync(string state, string cmd)
        {
            return await _ethereumBotContext.Commands.FirstOrDefaultAsync(u => u.State.State == state && u.Command == cmd) != null;
        }
            
        public async Task CreateUserAsync(int idUser)
        {
            var currMdl = await _ethereumBotContext.Currencies.FirstOrDefaultAsync(cur => cur.Currency == "USD");
            var exch = await _ethereumBotContext.Exchanges.FirstOrDefaultAsync(exh => exh.Exchange == "EXMO");
            var state =  await _ethereumBotContext.States.FirstOrDefaultAsync(st => st.State == "Start");
            
            var usr = new UserModel()
            {
                Id = idUser,
                Currency = currMdl,
                Exchange = exch,
                State = state
            };
            await _ethereumBotContext.Users.AddAsync(usr);
            await _ethereumBotContext.SaveChangesAsync();
        }

        public async Task<UserModel> GetUserAsync(int idUser)
            =>  await _ethereumBotContext.Users.FirstOrDefaultAsync(usr => usr.Id == idUser);
        
        public async Task<bool> IsExistUserAsync(int idUser)
            =>  await _ethereumBotContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == idUser) != null;
        
        public async Task<IEnumerable<CommandsModel>> GetListCommands(string state)
        {
            var query = (from cmd in _ethereumBotContext.Commands 
                        where cmd.State.State == state
                        select cmd);
            return await query.ToListAsync();
        }

        public async Task<string> GetTextCommand(string command, string state)
        {
            return (await _ethereumBotContext.Commands.FirstOrDefaultAsync(cmd => cmd.Command == command
                && cmd.State.State == state)).Text.Text;
        }

        public async Task<StateModel> GetStateAsync(string state)
            => await _ethereumBotContext.States.FirstOrDefaultAsync(st => st.State == state);

        public async Task SetNewState(UserModel user, StateModel state)
        {
            user.State = state;
            await _ethereumBotContext.SaveChangesAsync();
        }
    }
}