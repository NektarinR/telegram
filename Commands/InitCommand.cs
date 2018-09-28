using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ether_bot.Interfaces;
using Ether_bot.Models;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace Ether_bot.Commands
{
    public class InitCommand : ICommand
    {
        private readonly IStorageService _storageService;
        private readonly IExchangeService _exchangeService;
        public InitCommand(IStorageService storageService, IExchangeService exchangeService)
        {
            _storageService = storageService;
            _exchangeService = exchangeService;
        }

        public async Task ExecuteAsync(IBotService botService, Update update)
        {
            if (!await _storageService.IsExistUserAsync(update.Message.From.Id))
            {
                await _storageService.CreateUserAsync(update.Message.From.Id);                            
            }
            var user = await _storageService.GetUserAsync(update.Message.From.Id);
            var rate = await _exchangeService.GetRateAsync(user.Exchange);
            var text = await _storageService.GetTextCommand("GetCurrentRate", "Start");
            var resultText = String.Format(text, user.Exchange.Exchange, decimal.Round(rate.Value,2),
                DateTime.Now.ToUniversalTime());
            var commnds = await _storageService.GetListCommands("Start");
            List<InlineKeyboardButton> btn = new List<InlineKeyboardButton>();
            List<InlineKeyboardButton> lstBtns = new List<InlineKeyboardButton>();
            int i = 0;
            foreach (var tmpBtn in commnds)
            {
                btn.Add(new InlineKeyboardButton(){
                    CallbackData = tmpBtn.Command,
                    Text = tmpBtn.Command
                });
                if (btn.Count == 2)
                {
                    lstBtns.AddRange(btn);
                    btn.Clear();
                }
            }
            var keyboard = new InlineKeyboardMarkup(lstBtns);
            await botService.TlgBotClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: resultText,
                replyMarkup: keyboard
            );
        }
    }
}