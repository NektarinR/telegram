using System;
using System.Net;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using MihaZupan;
using System.Threading.Tasks;

namespace Ether_bot.Services
{
    public class EthereumBotService:IBotService
    {
        public TelegramBotClient TlgBotClient{get;}
        private readonly BotSettings _botSettings;
        
        public EthereumBotService(IOptions<BotSettings> botSettings)
        {            
            _botSettings = botSettings.Value;
            //var proxy = new WebProxy(_botSettings.Host)
            TlgBotClient = string.IsNullOrEmpty(_botSettings.Host)
                ? new TelegramBotClient(_botSettings.Token)
                : new TelegramBotClient(_botSettings.Token, 
                    new HttpToSocks5Proxy(_botSettings.Host, int.Parse(_botSettings.Port))
                );
            //TlgBotClient.SetWebhookAsync(_botSettings.WebHook).Wait();
        }
    }
}