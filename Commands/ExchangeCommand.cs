using System;
using System.Threading.Tasks;
using Ether_bot.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Ether_bot.Commands
{
    public class ExchangeCommand :ICommand
    {
        private readonly IStorageService _storageService;

        private IKeyboard _keyboad = new CallbackKeyboard(); 
        public ExchangeCommand(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task ExecuteAsync(IBotService botService, Update update)
        {
            var tskUser = _storageService.GetUserAsync(update.CallbackQuery.From.Id);
            var user = await tskUser;
            var text = await _storageService.GetTextCommandAsync(update.CallbackQuery.Data);
            var resultText = String.Format(text, user.Currency.Currency, user.Exchange.Exchange);
            var req = botService.TlgBotClient.EditMessageTextAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                messageId:update.CallbackQuery.Message.MessageId,
                parseMode: ParseMode.Html,
                text: resultText,
                replyMarkup: await _keyboad.GetKeyboardAsync(update.CallbackQuery.Data, _storageService)
            );        
            await req;
        }
    }
}