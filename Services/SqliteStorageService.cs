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
            
        public async Task CreateUserAsync(int idUser)
        {
            var currMdl = await _ethereumBotContext.Currencies.FirstOrDefaultAsync(cur => cur.Currency == "USD");
            var exch = await _ethereumBotContext.Exchanges.FirstOrDefaultAsync(exh => exh.Exchange == "Exmo");
            
            var usr = new UserModel()
            {
                Id = idUser,
                Currency = currMdl,
                Exchange = exch
            };
            await _ethereumBotContext.Users.AddAsync(usr);
            await _ethereumBotContext.SaveChangesAsync();
        }

        public async Task<UserModel> GetUserAsync(int idUser)
            =>  await _ethereumBotContext.Users.FirstOrDefaultAsync(usr => usr.Id == idUser);
        
        public async Task<bool> IsExistUserAsync(int idUser)
            =>  await _ethereumBotContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == idUser) != null;
        
        public async Task<IEnumerable<CommandsModel>> GetListCommandsAsync(string command)
        {
            var query = (from cmd in _ethereumBotContext.TreeCommands 
                        where cmd.ParentCommand.Data == command
                        select cmd.Command);
            
            return query.Any() ? await query.ToListAsync() : null;
        }

        public async Task<string> GetTextCommandAsync(string command)
        {
            var text = await _ethereumBotContext.Commands.FirstOrDefaultAsync(cmd => cmd.Data == command);
            return text == null ? null : text.Text.Text;
        }

        public async Task SetNewCurrencyAsync(UserModel user, CurrencyModel currency)
        {
            if (user.Currency != currency)
            {   
                user.Currency = currency;
                await _ethereumBotContext.SaveChangesAsync();
            }
        }

        public async Task SetNewExchangeAsync(UserModel user, ExchangeModel exchange)
        {
            if (user.Exchange != exchange)
            {
                user.Exchange = exchange;
                await _ethereumBotContext.SaveChangesAsync();
            }
        }

        public async Task<CurrencyModel> GetCurrencyAsync(string currency)
        {
            var curency = await _ethereumBotContext.Currencies.FirstOrDefaultAsync(cu => cu.Currency == currency);
            return curency;
        }

        public async Task<ExchangeModel> GetExchangeAsync(string exchange)
        {
            var excha = await _ethereumBotContext.Exchanges.FirstOrDefaultAsync(exch => exch.Exchange == exchange);
            return excha;
        }


    }
}