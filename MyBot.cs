using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot
{
    public class MyBot
        {
        private  TelegramBotClient TlgBotClient;
        private readonly BotSettings _botSettings;
        private readonly string NameBot;

        public MyBot()
        { 
            _botSettings = new BotSettings();
            
            WebProxy webProxy = new WebProxy("193.106.192.149:53281");
            
            //webProxy.Credentials = ne""w NetworkCredential("","");
            TlgBotClient =  new TelegramBotClient(
                "657927521:AAEF7Y9QlMCk5fNSHgzXP3G9Y6AGb4O_k_U",
                webProxy);
            NameBot = TlgBotClient.GetMeAsync().Result.Username;
            TlgBotClient.OnMessage += MyBotOnMessageReceived;
            TlgBotClient.OnCallbackQuery += MyBotOnCallbackQuery;
        }

        public void Start()
        {        
            Console.WriteLine($"Бот {NameBot} начал работать");
            TlgBotClient.StartReceiving(Array.Empty<UpdateType>());
            Console.ReadLine();
        }
        private async void MyBotOnMessageReceived(object sender, MessageEventArgs e)
        {        
            var message = e.Message;
            if (message == null || message.Type != MessageType.Text){
                await TlgBotClient.SendTextMessageAsync(
                    chatId: message.From.Id,
                    text: "Бот принимает только текст"
                );
                return;
            }
            switch (message.Text.Split(' ').First()){
                case "/start":
                    Console.WriteLine($"Пришло сообщения от:{message.From.Username} - /start, ");
                    InlineKeyboardMarkup keyBoard = new InlineKeyboardMarkup(new[]{
                        new[]{
                            InlineKeyboardButton.WithCallbackData("Валюта","1"),
                            InlineKeyboardButton.WithCallbackData("Биржа","2"),        
                        },
                        new[]{
                        InlineKeyboardButton.WithCallbackData("Период обноваления","3")
                        }
                    });
                        
                    await TlgBotClient.SendTextMessageAsync(
                        chatId: message.From.Id,
                        text: @"Вас приветсвует бот для эфира. Валюта - USD, биржа - exmo.me,
                        период обноваления - нету",
                        replyMarkup: keyBoard
                    );
                    break;
            }
        }
        private async void MyBotOnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            Console.WriteLine($"{e.CallbackQuery.Message}");
            await TlgBotClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.From.Id,
                        text: $@"{e.CallbackQuery.Id}- {e.CallbackQuery.InlineMessageId}"
                    );
        }
    }
}