using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Data
{
    public enum ChatType : int
    {
        Notice1 = 1,
        Notice2,
        Lobby_ToChannel = 3,//3
        Room_ToAll = 4,
        Room_ToTeam = 5,
        Whisper =6 ,
        Lobby_ToAll = 8,
        Clan
    }
}
