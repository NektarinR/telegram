using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ether_bot.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.Commands
{
    public class CurrencyCommand :ICommand
    {
        private readonly IStorageService _storageService;
        private readonly IExchangeService _exchangeService;

        private IKeyboard _keyboad = new CallbackKeyboard(); 
        public CurrencyCommand(IStorageService storageService, IExchangeService exchangeService)
        {
            _storageService = storageService;
            _exchangeService = exchangeService;
        }

        public async Task ExecuteAsync(IBotService botService, Update update)
        {
            var tskGetState = _storageService.GetStateAsync(update.CallbackQuery.Data);
            var tskUser = _storageService.GetUserAsync(update.CallbackQuery.From.Id);
            var user = await tskUser;
            var text = await _storageService.GetTextCommand(update.CallbackQuery.Data, user.State.State);
            var resultText = String.Format(text, user.Currency.Currency, user.Exchange.Exchange);            
            var req = botService.TlgBotClient.EditMessageTextAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                messageId:update.CallbackQuery.Message.MessageId,
                text: resultText,
                replyMarkup: await _keyboad.GetKeyboardAsync(await tskGetState, _storageService)
            );        
            await req;
            if (req.IsCompletedSuccessfully)    
                await _storageService.SetNewState(user, await tskGetState);
        }
    }
}