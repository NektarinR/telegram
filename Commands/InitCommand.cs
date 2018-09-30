using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ether_bot.Interfaces;
using Ether_bot.Models;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
            var user = await _storageService.GetUserAsync(update.Message.From.Id);
            var rate = await _exchangeService.GetRateAsync(user.Exchange);
            var text = await _storageService.GetTextCommandAsync("UpdateRate");
            var resultText = String.Format(text, user.Exchange.Exchange, decimal.Round(rate.Value,2),
                user.Currency.Currency, DateTime.Now.ToUniversalTime());
            await botService.TlgBotClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                parseMode: ParseMode.Html,
                text: resultText,
                replyMarkup: await _keyboard.GetKeyboardAsync("Init", _storageService)
            );
        }
    }
}