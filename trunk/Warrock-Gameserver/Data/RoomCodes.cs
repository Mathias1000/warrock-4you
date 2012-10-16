using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Data
{
    public enum RoomErrCode : int
    {
        GenericError = 94010,
        InvalidPassword = 94030,
        BadLevel = 94300,
        OnlyPremium = 94301
    }
}
