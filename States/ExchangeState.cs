using System.Threading.Tasks;

namespace Ether_bot.States
{
    public class ExchangeState : IUserState
    {

        public ExchangeState(UserStateService UserStateService)
        {

        }

        public Task ActionAsync(Task task)
        {
            throw new System.NotImplementedException();
        }

        public async Task GetCurrentRateAsync()
        {
            //throw new System.NotImplementedException();
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

        public Task ReturnAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}