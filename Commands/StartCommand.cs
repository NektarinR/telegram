using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ether_bot.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.Commands
{
    public class StartCommand :ICommand
    {
        private readonly IStorageService _storageService;
        private readonly IExchangeService _exchangeService;
        public StartCommand(IStorageService storageService, IExchangeService exchangeService)
        {
            _storageService = storageService;
            _exchangeService = exchangeService;
        }

        public async Task ExecuteAsync(IBotService botService, Update update)
        {
            var tskGetState = _storageService.GetStateAsync("Start");
            var user = await _storageService.GetUserAsync(update.CallbackQuery.From.Id);
            var rate = await _exchangeService.GetRateAsync(user.Exchange);
            var text = await _storageService.GetTextCommand(update.CallbackQuery.Data, user.State.State);
            var resultText = String.Format(text, user.Exchange.Exchange, decimal.Round(rate.Value,2),
                DateTime.Now.ToUniversalTime());
            var commnds = await _storageService.GetListCommands("Start");
            List<InlineKeyboardButton> btn = new List<InlineKeyboardButton>();
            List<InlineKeyboardButton> lstBtns = new List<InlineKeyboardButton>();
            int i = 0;
            foreach (var tmpBtn in commnds)
            {
                lstBtns.Add(new InlineKeyboardButton(){
                    CallbackData = tmpBtn.Command,
                    Text = tmpBtn.Command
                });
            }
            var keyboard = new InlineKeyboardMarkup(lstBtns);
            var req = botService.TlgBotClient.EditMessageTextAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                messageId: update.CallbackQuery.Message.MessageId,
                text: resultText,
                replyMarkup: keyboard
            );
            await req;
            if (req.IsCompletedSuccessfully)
                await _storageService.SetNewState(user, await tskGetState);
        }
    }
}