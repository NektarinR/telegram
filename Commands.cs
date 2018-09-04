using System;
namespace Ether_bot
{
    public enum Commands
    {
        
        #region Currency
        GoToCurrency = 0,
        SetUsd = 10,
        SetRub = 20,
        #endregion
        #region Exchange
        GoToExchange = 1,
        SetExmo = 11,
        #endregion
        #region TimeUpdate
        GoToTimeNotify = 2,
        Set15Minutes = 12,
        Set30Minutes = 22,
        Set60Minutes = 32,
        SetMyMinutes = 42,
        #endregion
        Back = 99

    }
}