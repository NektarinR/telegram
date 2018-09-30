
using System;
using Ether_bot.Interfaces;
using Ether_bot.Commands;

namespace Ether_bot.Services
{
    public static class FindCommand
    {
        public static ICommand Identify(string cmd, IStorageService storageService, 
            IExchangeService exchService)
        {
            switch (cmd)
            {
                case "Init":
                    return new InitCommand(storageService, exchService);
                case "GoToSettings":
                    return new SettingCommand(storageService);
                case "BackToStart":
                    return new StartCommand(storageService, exchService);
                case "UpdateRate":
                    return new GetCurrentRateCommand(storageService, exchService);
                case "GoToCurrency":
                    return new CurrencyCommand(storageService);
                case "USD":case "RUB":
                    return new SetCurrencyCommand(storageService, cmd);
                case "GoToExchanges":
                    return new ExchangeCommand(storageService);
                case "Binance":case "Exmo":
                    return new SetExchangeCommand(storageService, cmd);
                case "BackToSettings":
                    return new SettingCommand(storageService); 
                default:
                    return null;
            }
        }
    }
}