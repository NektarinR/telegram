using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ether_bot.Models;
using Ether_bot.States;
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
            
            UserStateService userStateSrv = new UserStateService();
            switch (update.Type)
            {
                case (UpdateType.Message):
                    await userStateSrv.InitStartStateAsync();
                break;
                case (UpdateType.CallbackQuery):
                    await userStateSrv.SetStateAsync(await userStateSrv.GetStateAsync(update.CallbackQuery.From.Id));
                    if (userStateSrv.IsCanExecute(update.CallbackQuery.Data))
                        userStateSrv.Execute();
                    //Приходит некоторая команда. в зависимости от нее мы можешь получить
                    //Либо перейти в новое состояние, либо исполни команду в этом состоянии
                    //
                    // if (состояние == start && команда == GetCurrentRateAsync) Обновить на главной странице курс
                break;
            }
        }
    }
}