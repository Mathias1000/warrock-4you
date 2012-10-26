using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Lib
{
    public enum LoginServerOpcodes : int
    {
        SendPatchVersion = 4112,
        LoginResponse =  4352,
    }
    public enum GameServerOpcodes : int
    {
        Welcome = 24832,
        PlayerInfo = 25088,
        SendUserList = 28960,
        ChannelChange = 28673,
        Room_List = 29184,
        CreateRoomSuccess = 29440,
        LeaveRoom = 29504,
        UpdateRoomPlayers = 29952,
        PlayerJoinRoom = 29456,
        Chat_Message = 29696,
        RoomAtion_Response = 30000,
        GameSpawnTick = 30016,
        ItemShopResult = 30208,
        Show_Message = 31264,
    }
}
