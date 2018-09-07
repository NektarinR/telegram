using System;
using System.Threading.Tasks;

namespace Ether_bot.Services
{
    public interface IExchangeService
    {
        Task<decimal?> GetRateAsync(string pair, string exchange);
    }
}