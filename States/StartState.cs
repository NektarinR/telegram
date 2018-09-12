using System.Threading.Tasks;

namespace Ether_bot.States
{
    public class StartState : IUserState
    {
        private readonly UserStateService _userStateService;
        public StartState(UserStateService userStateService)
        {
            _userStateService = userStateService;
        }
        public async Task GoToCurrencyAsync()
        {}        
        public async Task GoToExchangeAsync()
        {}
        public async Task GoToSettingsAsync()
        {
            await _userStateService.SetStateAsync(_userStateService.Settings);
        }
        public async Task GoToTimeNotifyAsync()
        {}
        public async Task ReturnAsync()
        {}
    }
}