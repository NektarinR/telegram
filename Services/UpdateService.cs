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
                        ICommand command = FindCommand.Identify("Init", _storageService, _exchangeService);
                        await command.ExecuteAsync(_botService, update);
                    }
                break;
                case (UpdateType.CallbackQuery):
                    var user = await _storageService.GetUserAsync(update.CallbackQuery.From.Id);
                    var cmd = update.CallbackQuery.Data;
                    if (await _storageService.CanExecuteAsync(user.State.State, cmd))
                    {
                        var command = FindCommand.Identify(cmd, _storageService, _exchangeService);
                        if (command != null)
                            await command?.ExecuteAsync(_botService, update);
                    }
                break;
            }
        }
    }
}