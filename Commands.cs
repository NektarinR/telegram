using System;
namespace Ether_bot
{
    public enum Commands
    {
        
        #region Currency
        SetUsd = 10,
        SetRub = 20,
        SetEur = 30, 
        #endregion
        #region Exchange
        SetExmo = 11,
        SetBinance = 21,
        SetOkex = 31,
        SetBitfinex = 41,
        #endregion
        #region TimeUpdate
        Set15Minutes = 12,
        Set30Minutes = 22,
        Set60Minutes = 32,
        SetMyMinutes = 42,
        #endregion
        Back = 99

    }
}