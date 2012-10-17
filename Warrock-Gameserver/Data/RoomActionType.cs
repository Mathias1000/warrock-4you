using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Data
{
    public enum RoomActionType : ushort
    {
        InviteIntoGame = 1,
        ChangeRdy = 50,
        ChangeMapID = 51,
        ChangeRoomMode = 52,
        ChangeRoomRounds = 53,
        ChangeRoomTimeLeft = 54,
        ChangeKillLimit = 55,
        ChangeRoomSlot = 56,
        ChangeRoomPing = 59,
        ChangeRoomAutoStart = 62

    }
}
