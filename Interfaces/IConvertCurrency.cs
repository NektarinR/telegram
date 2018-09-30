using System;
using System.Threading.Tasks;
using Ether_bot.Models;

namespace Ether_bot.Interfaces
{
    public interface IConvertCurrency
    {
        Task<decimal> ConvertCurrencyAsync(decimal value, CurrencyModel currency);
    }
}