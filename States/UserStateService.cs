using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ether_bot.States
{
    public class UserStateService
    {
        public StartState Start {get; private set;}
        public SettingsState Settings {get; private set;}
        //public CurrencyState Currency {get;private set;}
        //public ExchangeState Exchange {get; private set;}
        private IUserState _userState;
        private int[,] stepMatrix = new int[10,10];

        public UserStateService()
        {
            Start = new StartState(this);
            Settings = new SettingsState(this);
            _userState = Start;
        }

        public async Task InitStartStateAsync()
        {
            //Создать клиента в БД, если не существует иначе получить из БД 
            //Установить ему состояние как Start
            await SetStateAsync(new StartState(this));
            //клавиатура подргружается из БД
            //Выдать Text и Клавиатуру
        }

        public async Task SetStateAsync(IUserState userState)
        {            
            _userState = userState;
            //Сохранить состояние в БД
        }

        public bool IsCanExecute(string Command)
        {
            Commands cmnd = Enum.Parse<Commands>(Command);
        }

        public async Task<IUserState> GetStateAsync(int idUser)
        {
            //получить из БД состояние 
            //вернуть соответствующий объект
            throw new NotImplementedException();
        }        
        public async Task GoToSettingsAsync()
        {
            await _userState.GoToSettingsAsync();
        }
        public async Task  GoToCurrencyAsync()
        {
            await _userState.GoToSettingsAsync();
        }
        public async Task GoToExchangeAsync()
        {
            await _userState.GoToExchangeAsync();
        }
        public async Task GoToTimeNotifyAsync()
        {
            await _userState.GoToTimeNotifyAsync();
        }
        public async Task ReturnAsync()
        {
            await _userState.ReturnAsync();
        }
    }
}