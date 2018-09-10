using System;
using System.Net;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using MihaZupan;
using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.Services
{
    public class EthereumBotService:IBotService
    {
        public TelegramBotClient TlgBotClient{get;}
        private readonly BotSettings _botSettings;        
        private readonly Dictionary<States,ReplyKeyboardMarkup> dictKeyboard;
        
        public EthereumBotService(IOptions<BotSettings> botSettings)
        {            
            _botSettings = botSettings.Value;
            TlgBotClient = string.IsNullOrEmpty(_botSettings.HostProxy)
                ? new TelegramBotClient(_botSettings.Token)
                : new TelegramBotClient(_botSettings.Token, 
                    new HttpToSocks5Proxy(_botSettings.HostProxy, int.Parse(_botSettings.PortProxy))
                );
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
                new KeyboardButton[]{"Exmo"},
                new KeyboardButton[]{"Binance"},
                new KeyboardButton[]{"Назад"}
            },true));
            dictKeyboard.Add(States.TimeNotify, new ReplyKeyboardMarkup(new []
            {
                new KeyboardButton[]{"Назад", "15 мин"},
                new KeyboardButton[]{"30 мин", "60 мин"},
                new KeyboardButton[]{"Свое время","Не уведомлять"}
            },true));
        }

        public ReplyKeyboardMarkup GetKeyboardByState(States state) => dictKeyboard[state];
    }
}