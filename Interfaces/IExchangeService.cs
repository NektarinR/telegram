using System;
using System.Threading.Tasks;
using Ether_bot.Models;

namespace Ether_bot.Interfaces
{
    public interface IExchangeService
    {
        Task<decimal?> GetRateAsync(ExchangeModel exchange);
    }
}