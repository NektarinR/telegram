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
        
        private IKeyboard _keyboard = new CallbackKeyboard();

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
            var tskState = _storageService.GetStateAsync("Start");
            var user = await _storageService.GetUserAsync(update.Message.From.Id);
            var rate = await _exchangeService.GetRateAsync(user.Exchange);
            var text = await _storageService.GetTextCommand("Update", "Start");
            var resultText = String.Format(text, user.Exchange.Exchange, decimal.Round(rate.Value,2),
                DateTime.Now.ToUniversalTime());
            await botService.TlgBotClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: resultText,
                replyMarkup: await _keyboard.GetKeyboardAsync(await tskState, _storageService)
            );
        }
    }
}