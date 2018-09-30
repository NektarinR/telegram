using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ether_bot.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.Commands
{
    public class SettingCommand : ICommand
    {
        private readonly IStorageService _storageService;

        private IKeyboard _keyboard = new CallbackKeyboard();

        public SettingCommand(IStorageService storageService)
        {
            _storageService = storageService;
        }
        public async Task ExecuteAsync(IBotService botService, Update update)
        {
            var user = await _storageService.GetUserAsync(update.CallbackQuery.From.Id);
            var text = await _storageService.GetTextCommandAsync(update.CallbackQuery.Data);
            var resultText = String.Format(text, user.Currency.Currency, user.Exchange.Exchange);
            var keyboard = await _keyboard.GetKeyboardAsync(update.CallbackQuery.Data, _storageService);
            var req = botService.TlgBotClient.EditMessageTextAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                messageId:update.CallbackQuery.Message.MessageId,
                parseMode: ParseMode.Html,
                text: resultText,
                replyMarkup: keyboard
            );        
            await req;
        }
    }
}