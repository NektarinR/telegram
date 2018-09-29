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
        private readonly IExchangeService _exchangeService;

        private IKeyboard _keyboard = new CallbackKeyboard();

        public SettingCommand(IStorageService storageService, IExchangeService exchangeService)
        {
            _storageService = storageService;
            _exchangeService = exchangeService;
        }
        public async Task ExecuteAsync(IBotService botService, Update update)
        {
            var tskGetState = _storageService.GetStateAsync("Settings");
            var user = await _storageService.GetUserAsync(update.CallbackQuery.From.Id);
            var text = await _storageService.GetTextCommand(update.CallbackQuery.Data, user.State.State);
            var resultText = String.Format(text, user.Currency.Currency, user.Exchange.Exchange);
            var keyboard = await _keyboard.GetKeyboardAsync(await tskGetState, _storageService);
            var req = botService.TlgBotClient.EditMessageTextAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                messageId:update.CallbackQuery.Message.MessageId,
                text: resultText,
                replyMarkup: keyboard
            );        
            await req;
            if (req.IsCompletedSuccessfully)    
                await _storageService.SetNewState(user, await tskGetState);
        }
    }
}