using System;
using System.Collections.Generic;
using System.Net.Http;
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
        private readonly Dictionary<States,ReplyKeyboardMarkup> dictKeyboard;
        public UpdateService(IBotService botService, 
            ILogger<UpdateService> logger, IStorageService storageService)
        {
            _botService = botService;
            _logger = logger;
            _storageService = storageService;
            dictKeyboard = new Dictionary<States, ReplyKeyboardMarkup>();
            dictKeyboard.Add(States.Start,new ReplyKeyboardMarkup(new [] 
            {
                new KeyboardButton[] { "Текущий курс","Валюта"},
                new KeyboardButton[] { "Биржа", " Уведомления" }
            },true));
            dictKeyboard.Add(States.Currency, new ReplyKeyboardMarkup(new []
            {
                new KeyboardButton[]{"USD", "Rub"},
                new KeyboardButton[]{"Назад"}
            },true));
            dictKeyboard.Add(States.Exchange, new ReplyKeyboardMarkup(new []
            {
                new KeyboardButton[]{"exmo.me"},
                new KeyboardButton[]{"Назад"}
            },true));
            dictKeyboard.Add(States.TimeNotify, new ReplyKeyboardMarkup(new []
            {
                new KeyboardButton[]{"Назад", "15 мин"},
                new KeyboardButton[]{"30 мин", "60 мин"},
                new KeyboardButton[]{"Свое время","Не уведомлять"}
            },true));
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
                            msg.From.Username, msg.Date,msg.Chat.Id, $"{States.Start}");                        
                        await SendStartMenuAsync(msg,States.Start);
                    }else {
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
                    await SendNewKeyboadAsync(msg, States.Exchange,"Выберите биржу, с которой будет браться курс ethereum");
                break;
                case "Уведомления":
                    if (st != States.Start)
                        return;
                    await SendNewKeyboadAsync(msg, States.TimeNotify,"Выберите время уведомления или установите свое");
                break;
                case "Текущий курс":

                break;
                case "USD":case "RUB":

                break;
                case "Назад":
                    if (st==States.Currency || st==States.Exchange ||st==States.TimeNotify)
                        await SendStartMenuAsync(msg,States.Start);
                break;
            }
        }
        private async Task SendNewKeyboadAsync(Message msg, States state, string txt)
        {
            await _storageService.UpdateUserStateAsync(msg.From.Id, $"{state}",msg.Date,msg.MessageId);

            await _botService.TlgBotClient.SendTextMessageAsync(
                chatId:msg.Chat.Id,
                text:txt,
                replyMarkup: dictKeyboard[state]
            );
        }

        private async Task SendStartMenuAsync(Message msg, States state)
        {
            var setUsr = await _storageService.GetSettingsUserAsync(msg.From.Id);
            await _botService.TlgBotClient.SendTextMessageAsync(
            chatId:msg.Chat.Id,
            text: String.Format("Текущие настройки:\nвалюта - {0}\nбиржа - {1}\nвремя обновления -{2}",
                setUsr.currency,setUsr.exchange,setUsr.timeUpdate),
            replyMarkup: dictKeyboard[state]);
            await _storageService.UpdateUserStateAsync(msg.From.Id,$"{state}",msg.Date,msg.MessageId);
        }
    }
}