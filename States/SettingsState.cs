using System.Threading.Tasks;

namespace Ether_bot.States
{
    public class SettingsState : IUserState
    {
        private readonly UserStateService _userStateService;
        public SettingsState(UserStateService userStateService)
        {
            _userStateService = userStateService;
        }

        public async Task ActionAsync(Task task)
        {
            
        }

        public Task ActionAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task GetCurrentRateAsync()
        {
            
        }

        public Task GoToCurrencyAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task GoToExchangeAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task GoToSettingsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task GoToTimeNotifyAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task ReturnAsync()
        {
            await _userStateService.SetStateAsync(_userStateService.Start);
        }
    }
}