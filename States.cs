using System;

namespace Ether_bot
{
    public enum States
    {
        Start = 0,
        #region Currency
        Currency = 1,
        #endregion
        #region Exchange
        Exchange = 2,
        #endregion
        #region TimeUpdate
        TimeNotify = 3,
        SetTime = 13
        #endregion
    }  
} 