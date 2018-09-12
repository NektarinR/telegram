using System;
using System.Threading.Tasks;

namespace Ether_bot.States
{
    public interface ISomeAction
    {
        Task GetCurrentRateAsync();
    }
}