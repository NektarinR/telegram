using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ether_bot.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.Commands
{
    public class GetCurrentRateCommand : ICommand
    {
        private readonly IStorageService _storageService;
        private readonly IExchangeService _exchangeService;

        private readonly IConvertCurrency _convertCurrency;

        private IKeyboard _keyboard = new CallbackKeyboard();
        public GetCurrentRateCommand(IStorageService storageService, IExchangeService exchangeService)
        {
            _storageService = storageService;
            _exchangeService = exchangeService;
        }

        public async Task ExecuteAsync(IBotService botService, Update update)
        {
            var user = await _storageService.GetUserAsync(update.CallbackQuery.From.Id);
            var rate = await _exchangeService.GetRateAsync(user.Exchange);
            var text = await _storageService.GetTextCommandAsync(update.CallbackQuery.Data);
            var money = user.Currency.Currency=="USD" ? 
                rate.Value : 
                await _convertCurrency?.ConvertCurrencyAsync(rate.Value, user.Currency);
            var resultText = String.Format(text, user.Exchange.Exchange, decimal.Round(money,2),
                DateTime.Now.ToUniversalTime());
            await botService.TlgBotClient.EditMessageTextAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                messageId: update.CallbackQuery.Message.MessageId,
                parseMode: ParseMode.Html,
                text: resultText,
                replyMarkup: await _keyboard.GetKeyboardAsync(update.CallbackQuery.Data, _storageService)
            );
        }
    }
}