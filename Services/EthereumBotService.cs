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
        public TelegramBotClient TlgBotClient{get; private set;}
        private readonly BotSettings _botSettings;        
        
        public EthereumBotService(IOptions<BotSettings> botSettings)
        {            
            _botSettings = botSettings.Value;
            TlgBotClient = string.IsNullOrEmpty(_botSettings.HostProxy)
                ? new TelegramBotClient(_botSettings.Token)
                : new TelegramBotClient(_botSettings.Token, 
                    new HttpToSocks5Proxy(_botSettings.HostProxy, int.Parse(_botSettings.PortProxy))
                );
        }
    }
}