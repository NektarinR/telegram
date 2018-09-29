using System;
using System.Threading.Tasks;
using Ether_bot.Interfaces;
using Telegram.Bot.Types;

namespace Ether_bot.Commands
{
    public class SetCurrencyCommand : ICommand
    {
        private readonly IStorageService _storageService;
        private readonly IExchangeService _exchangeService;

        private readonly string _currency;
        private IKeyboard _keyboard = new CallbackKeyboard();
        public SetCurrencyCommand(IStorageService storageService, IExchangeService exchangeService, string currency)
        {
            _storageService = storageService;
            _exchangeService = exchangeService;
            _currency = currency;
        }

        public async Task ExecuteAsync(IBotService botService, Update update)
        {
            var user = await _storageService.GetUserAsync(update.CallbackQuery.From.Id);
            var text = await _storageService.GetTextCommand(update.CallbackQuery.Data, user.State.State);
            var resultText = String.Format(text, _currency, user.Exchange.Exchange);
            var keyboard = await _keyboard.GetKeyboardAsync(user.State, _storageService);
            await botService.TlgBotClient.EditMessageTextAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                messageId:update.CallbackQuery.Message.MessageId,
                text: resultText,
                replyMarkup: keyboard
            );        
        }
    }
}