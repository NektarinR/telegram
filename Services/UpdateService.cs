using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ether_bot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IBotService _botService;
        private readonly ILogger _logger;
        private readonly IStorageService _storageService;
        private readonly IExchangeService _exchangeService;
        public UpdateService(IBotService botService, ILogger<UpdateService> logger, 
            IStorageService storageService, IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
            _botService = botService;
            _logger = logger;
            _storageService = storageService;            
        }
        public async Task SendAnswerAsync(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Message:                
                    var msg = update.Message;
                    if (update.Message.Text == "/start")
                    {
                        await _storageService.CreateUserAsync(msg.From.Id,
                            msg.From.Username, msg.Date, msg.Chat.Id, $"{States.Start}");                        
                        await SendStartMenuAsync(msg,States.Start);
                    }
                    else 
                    {
                        await WhatToDoAsync(msg);
                    }
                break;
            }
        }

        private async Task WhatToDoAsync(Message msg)
        {                
            var curSt = await _storageService.GetUserStateAsync(msg.From.Id);
            if (curSt == null)
                return;
            States st;
            if (!Enum.TryParse(curSt.State,out st))
                return;
            switch (msg.Text)
            {
                case "Валюта":
                    if (st != States.Start)
                        return;
                    await SendNewKeyboadAsync(msg,States.Currency,"Выберите валюту, в которой будет ethereum");
                    break;
                case "Биржа":
                    if (st != States.Start)
                        return;
                    await SendNewKeyboadAsync(msg, States.Exchange,
                        "Выберите биржу, с которой будет браться курс ethereum");
                    break;
                case "Уведомления":
                    if (st != States.Start)
                        return;
                    await SendNewKeyboadAsync(msg, States.TimeNotify,
                        "Выберите время уведомления или установите свое");
                    break;
                case "Текущий курс":
                    if (st != States.Start)
                        return;
                    await SendCurrentRate(msg);
                    break;
                case "USD":case "RUB":
                    if (st != States.Currency)
                        return;
                    break;
                case "Exmo":case "Binance":
                    if (st != States.Exchange)
                        return;
                    string strNewExchange = msg.Text;
                    await _storageService.UpdateUserExchangeAsync(msg.From.Id, strNewExchange);
                    await _botService.TlgBotClient.SendTextMessageAsync(
                        chatId: msg.From.Id,
                        text: "Биржа изменена"
                    );
                    break;
                case "Назад":
                    if (st==States.Currency || st==States.Exchange ||st==States.TimeNotify)
                        await SendStartMenuAsync(msg,States.Start);
                    break;
            }
        }
        private async Task SendCurrentRate(Message msg)
        {
            var settingsUser = await _storageService.GetSettingsUserAsync(msg.From.Id);
            //var strPair = $"ETH_{settingsUser.currency.ToUpper()}";
            var rate = await _exchangeService.GetRateAsync(settingsUser.currency, settingsUser.exchange);
            await _botService.TlgBotClient.SendTextMessageAsync(
                chatId: msg.From.Id,
                text: rate == null
                    ?"На бирже не ведется учет в такой валюте"
                    :$"{rate} {settingsUser.currency}"
            );
        }
        private async Task SendNewKeyboadAsync(Message msg, States state, string txt)
        {
            await _storageService.UpdateUserStateAsync(msg.From.Id, $"{state}",msg.Date,msg.MessageId);
            await _botService.TlgBotClient.SendTextMessageAsync(
                chatId:msg.Chat.Id,
                text:txt,
                replyMarkup: _botService.GetKeyboardByState(state)
            );            
        }

        private async Task SendStartMenuAsync(Message msg, States state)
        {
            var setUsr = await _storageService.GetSettingsUserAsync(msg.From.Id);
            await _botService.TlgBotClient.SendTextMessageAsync(
            chatId: msg.Chat.Id,
                text: String.Format("Текущие настройки:\nвалюта - {0}\nбиржа - {1}\nвремя обновления -{2}",
                    setUsr.currency,setUsr.exchange,setUsr.timeUpdate),
                replyMarkup: _botService.GetKeyboardByState(state)
            );
            await _storageService.UpdateUserStateAsync(msg.From.Id,$"{state}",msg.Date,msg.MessageId);
        }
    }
}