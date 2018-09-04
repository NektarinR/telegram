using System;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.Services
{
    public class UserStateService:IUserState
    {
        private States State{get;set;}
        private readonly Lazy<Dictionary<States,ReplyKeyboardMarkup>> dictKeyboard;

        public UserStateService()
        {
            dictKeyboard = new Lazy<Dictionary<States, ReplyKeyboardMarkup>>();
            dictKeyboard.Value.Add(States.Start,new ReplyKeyboardMarkup(new [] 
            {
                new KeyboardButton[] { "Текущий курс","Валюта"},
                new KeyboardButton[] { "Биржа", " Уведомления" }
            }));
            dictKeyboard.Value.Add(States.Currency, new ReplyKeyboardMarkup(new []
            {
                new KeyboardButton[]{"USD", "Rub"},
                new KeyboardButton[]{"Назад"}
            }));
            dictKeyboard.Value.Add(States.Exchange, new ReplyKeyboardMarkup(new []
            {
                new KeyboardButton[]{"exmo.me"},
                new KeyboardButton[]{"Назад"}
            }));
            dictKeyboard.Value.Add(States.TimeNotify, new ReplyKeyboardMarkup(new []
            {
                new KeyboardButton[]{"Назад", "15 мин"},
                new KeyboardButton[]{"30 мин", "60"},
                new KeyboardButton[]{"Свое время"}
            }));
        }

        public ReplyKeyboardMarkup Execute()
        {
            if (State == States.SetTime)
                return null;
            return dictKeyboard.Value[State];
        }

        public bool CanExecute(Commands commnd)
        {
            switch (State)
            {
                case States.Start:
                    if (commnd != Commands.GoToCurrency && 
                        commnd != Commands.GoToExchange && commnd !=Commands.GoToTimeNotify)
                        return false;
                    return true;
                case States.Currency:
                    if (commnd != Commands.SetRub && commnd != Commands.SetUsd && commnd != Commands.Back)
                        return false;
                    return true;
                case States.Exchange:
                    if (commnd != Commands.SetExmo && commnd != Commands.Back)
                        return false;
                    return true;                
                case States.TimeNotify:
                    if (commnd != Commands.Set15Minutes && commnd != Commands.Set30Minutes 
                        && commnd != Commands.Set60Minutes &&commnd != Commands.SetMyMinutes
                        && commnd != Commands.Back)
                        return false;
                    return true;
                case States.SetTime:
                    return false;
                default:
                    return false;
            }
        }
    }
}