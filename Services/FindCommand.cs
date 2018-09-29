
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
                case "Settings":
                    return new SettingCommand(storageService, exchService);
                case "Start":
                    return new StartCommand(storageService, exchService);
                case "Update":
                    return new GetCurrentRateCommand(storageService, exchService);
                case "Currency":
                    return new CurrencyCommand(storageService, exchService);
                case "USD":case "RUB":
                    return new SetCurrencyCommand(storageService, exchService, cmd);
                default:
                    return null;
            }
        }
    }
}