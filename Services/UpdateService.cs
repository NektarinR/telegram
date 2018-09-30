using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ether_bot.Models;
using Ether_bot.Interfaces;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IBotService _botService;
        private readonly IStorageService _storageService;
        private readonly IExchangeService _exchangeService;

        public UpdateService(IBotService botService, IStorageService storageService, 
            IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
            _botService = botService;
            _storageService = storageService;            
        }
        public async Task SendAnswerAsync(Update update)
        {                        
            switch (update.Type)
            {
                case (UpdateType.Message):
                    if (update.Message.Text == "/start")
                    {
                        ICommand com = FindCommand.Identify("Init", _storageService, _exchangeService);
                        await com.ExecuteAsync(_botService, update);
                    }
                break;
                case (UpdateType.CallbackQuery):
                    var cmd = update.CallbackQuery.Data;
                    var command = FindCommand.Identify(cmd, _storageService, _exchangeService);
                    if (command != null)
                        await command.ExecuteAsync(_botService, update);
                break;
            }
        }
    }
}