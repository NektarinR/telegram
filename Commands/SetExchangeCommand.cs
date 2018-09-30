using System;
using System.Threading.Tasks;
using Ether_bot.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Ether_bot.Commands
{
    public class SetExchangeCommand:ICommand
    {
        private readonly IStorageService _storageService;
        private readonly string _exchange;
        private IKeyboard _keyboard = new CallbackKeyboard();

        public SetExchangeCommand(IStorageService storageService, string exchange)
        {
            _storageService = storageService;
            _exchange = exchange;
        }
        public async Task ExecuteAsync(IBotService botService, Update update)
        {
            var user = await _storageService.GetUserAsync(update.CallbackQuery.From.Id);
            if (user.Exchange.Exchange != _exchange)
            {
                var text = await _storageService.GetTextCommandAsync(update.CallbackQuery.Data);
                var resultText = String.Format(text, user.Currency.Currency, _exchange);
                var keyboard = await _keyboard.GetKeyboardAsync(update.CallbackQuery.Data, _storageService);
                var tskChangeExchange =  botService.TlgBotClient.EditMessageTextAsync(
                    chatId: update.CallbackQuery.Message.Chat.Id,
                    messageId: update.CallbackQuery.Message.MessageId,
                    parseMode: ParseMode.Html,
                    text: resultText,
                    replyMarkup: keyboard
                );
                await tskChangeExchange;
                if (tskChangeExchange.IsCompletedSuccessfully)
                {
                    await _storageService.SetNewExchangeAsync(user, await _storageService.GetExchangeAsync(_exchange));
                }
            }
        }
    }
}