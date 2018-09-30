using System;
using System.Threading.Tasks;
using Ether_bot.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Ether_bot.Commands
{
    public class SetCurrencyCommand : ICommand
    {
        private readonly IStorageService _storageService;

        private readonly string _currency;
        private IKeyboard _keyboard = new CallbackKeyboard();
        public SetCurrencyCommand(IStorageService storageService, string currency)
        {
            _storageService = storageService;
            _currency = currency;
        }

        public async Task ExecuteAsync(IBotService botService, Update update)
        {
            var user = await _storageService.GetUserAsync(update.CallbackQuery.From.Id);
            if (user.Currency.Currency != _currency)
            {
                var text = await _storageService.GetTextCommandAsync(update.CallbackQuery.Data);
                var resultText = String.Format(text, _currency, user.Exchange.Exchange);
                var keyboard = await _keyboard.GetKeyboardAsync(update.CallbackQuery.Data, _storageService);
                var tskChangeCurr = botService.TlgBotClient.EditMessageTextAsync(
                    chatId: update.CallbackQuery.Message.Chat.Id,
                    messageId: update.CallbackQuery.Message.MessageId,
                    parseMode: ParseMode.Html,
                    text: resultText,
                    replyMarkup: keyboard
                );
                await tskChangeCurr;
                if (tskChangeCurr.IsCompletedSuccessfully)
                {
                    await _storageService.SetNewCurrencyAsync(user, await _storageService.GetCurrencyAsync(_currency));
                }
            }
        }
    }
}