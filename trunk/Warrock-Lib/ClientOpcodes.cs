using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Lib
{
    public enum ClientLoginOpcodes : int
    {
        PatchRequest    = 4112,
        Login = 60001,
       // Login           = 4352, offical opcode
        ChangeNickname  = 4353,

    }
    public enum ClientGameOpcode : int//optimzide later size
    {
        GetPlayerInfo = 25088,
        ChangeChannel = 28673,
        UserListRequest = 28960,
        RoomList_Request = 29184,
        Create_Room = 29440,
        Join_Room = 29456,
        Leave_Room = 29504,
        ChangeEquip = 29970,
        ChangeRoomData = 30000,
        ItemShopBuyItem = 30208,
        ClientChatMessage = 29696,
       // Welcome = 91337, offical opcode
        Welcome = 60002,
    }
}
